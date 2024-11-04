using BattleshipsDP.Client;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary;
using SharedLibrary.Bridge;
using System.Numerics;
using SharedLibrary.Builder;
using BattleshipsDP.Client.Pages;
using System.IO;

namespace BattleshipsDP.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _gameService;

        // Retrieve the singleton instance of GameService
        public GameHub()
        {
            _gameService = GameService.Instance; // Access the singleton instance directly
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Player with ID {Context.ConnectionId} connected.");
            await Clients.Caller.SendAsync("Rooms", _gameService.GetAllRooms().OrderBy(r => r.RoomName));
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Player with ID {Context.ConnectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<GameRoom> CreateRoom(string name, string playerName, string difficulty)
        {
            // Create the room via GameService
            GameRoom room = _gameService.CreateRoom(name, difficulty);

            // Add the player to the room
            var newPlayer = new Player(Context.ConnectionId, playerName);
            _gameService.TryAddPlayerToRoom(room.RoomId, newPlayer);

            // Add the player to the appropriate group and notify clients
            await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);
            await Clients.All.SendAsync("Rooms", _gameService.GetAllRooms().OrderBy(r => r.RoomName));

            return room;
        }

        public async Task<GameRoom?> JoinRoom(string roomId, string playerName)
        {
            var newPlayer = new Player(Context.ConnectionId, playerName);
            if (_gameService.TryAddPlayerToRoom(roomId, newPlayer))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Group(roomId).SendAsync("PlayerJoined", newPlayer);
                await Clients.All.SendAsync("Rooms", _gameService.GetAllRooms().OrderBy(r => r.RoomName));
                return _gameService.GetRoomById(roomId);
            }

            return null;
        }

        public async Task<GameRoom> GetRoomById(string roomId) => _gameService.GetRoomById(roomId);

        
        public async Task<int> GetBoardSize(string playerId)
        {
            GameRoom room = _gameService.GetRoomByPlayerId(playerId);
            if (room.Difficulty == "Easy") return await Task.FromResult(8);
            else if (room.Difficulty == "Medium") return await Task.FromResult(10);
            else return await Task.FromResult(12);
        }

        public async Task<int> GetBig(string playerId)
        {
            var room = _gameService.GetRoomByPlayerId(playerId);
            var team = room.Game.GetTeamByPlayer(playerId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new BigShot().GetType());
            return amount;
        }
        public async Task<int> GetPiercer(string playerId)
        {
            var room = _gameService.GetRoomByPlayerId(playerId);
            var team = room.Game.GetTeamByPlayer(playerId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new PiercerShot().GetType());
            return amount;
        }
        public async Task<int> GetSlasher(string playerId)
        {
            var room = _gameService.GetRoomByPlayerId(playerId);
            var team = room.Game.GetTeamByPlayer(playerId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new SlasherShot().GetType());
            return amount;
        }
        public async Task<int> GetCross(string playerId)
        {
            var room = _gameService.GetRoomByPlayerId(playerId);
            var team = room.Game.GetTeamByPlayer(playerId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new CrossShot().GetType());
            return amount;
        }

        public async Task StartGame(string roomId)
        {
            var room = _gameService.GetRoomById(roomId);
            if (room != null)
            {
                room.Game.StartGame();

                await Clients.Group(roomId).SendAsync("NavigateToBattle");
                await Task.Delay(500);

                foreach (var player in room.Players)
                {
                    var team = room.Game.GetTeamByPlayer(player.ConnectionId);
                    await Clients.Client(player.ConnectionId).SendAsync(
                        "ReceivePlayerInfo",
                        player.Name,
                        player.ConnectionId,
                        team,
                        player.IsTeamLeader);

                }

            }
        }

        public async Task PlayerReady()
        {
            Console.WriteLine("Player is ready.");
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);

            if (room == null) return;

            var game = room.Game;
            var player = room.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null) return;

            // Register player as a turn observer
            game.RegisterTurnObserver(new TurnObserver(player.ConnectionId, Clients));

            var team = game.GetTeamByPlayer(connectionId);

            // Determine team leader status based on the connection ID matching the team's first player
            bool isTeamLeader = (team == "Team A" && connectionId == game.ATeamPlayer1Id) ||
                               (team == "Team B" && connectionId == game.BTeamPlayer1Id);

            // Update the player's team leader status
            player.IsTeamLeader = isTeamLeader;

            // Send player info including team leader status
            await Clients.Client(connectionId).SendAsync(
                "ReceivePlayerInfo",
                player.Name,
                connectionId,
                team,
                isTeamLeader);
            await NotifyBigRemaining(room, connectionId);
            await NotifyPiercerRemaining(room, connectionId);
            await NotifySlasherRemaining(room, connectionId);
            await NotifyCrossRemaining(room, connectionId);
        }

        public async Task HighlightBlockForTeam(int row, int col)
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);

            if (room == null || !room.Game.GameStarted || room.Game.GameOver) return;
            if (room.Game.CurrentPlayerId != connectionId) return; // Only allow current player to highlight

            var teammates = room.Game.GetTeammates(connectionId);
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceiveBlockHighlight", row, col);
            }
        }

        public async Task ShootAtOpponent(int row, int col, string type)
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);
            IShotCollection shot;
            if (room == null || !room.Game.GameStarted || room.Game.GameOver) return;

            if(type == "Simple")
            {
                shot = new SimpleShot();
            }
            else if(type == "Big")
            {
                shot = new BigShot();
            }
            else if (type =="Slasher")
            {
                shot = new SlasherShot();
            }
            else if (type == "Piercer")
            {
                shot = new PiercerShot();
            }
            else if (type == "Cross")
            {
                shot = new CrossShot();
            }
            else
            {
                shot = null;
            }

            if (room.Game.CurrentPlayerId != connectionId)
            {
                await Clients.Client(connectionId).SendAsync("NotYourTurn");
                return;
            }

            var player = room.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            var game = room.Game;
            var teamName = game.GetTeamByPlayer(connectionId);
            Team team = room.Game.GetTeamByPlayer(connectionId) == "Team A" ? room.Game.ATeam : room.Game.BTeam; ;

            //var shot = player?.(shotType);
            bool shotmade;
            if(shot.GetType().Equals(new SimpleShot().GetType()))
            {
                shotmade = true;
            }
            else
            {
                shotmade = team.TakeShot(shot.GetType());
            }
            if (shot == null || !shotmade)
            {
                await Clients.Client(connectionId).SendAsync("InvalidShotType");
                room.Game.UpdateTurn();
                return;
            }
            if (shotmade)
            {
                var coordinates = shot.GetSpread(row, col);
                bool isGameOver = false;

                foreach (var hit in coordinates)
                {
                    string result = room.Game.ShootCell(hit.Item1, hit.Item2, connectionId, out isGameOver);

                    if (result == "already_shot")
                    {
                        await Clients.Client(connectionId).SendAsync("AlreadyShot", hit.Item1, hit.Item2);
                        continue;
                    }

                    await NotifyTeammatesOfShot(room, connectionId, hit, result);
                    await NotifyOpponentsOfShot(room, connectionId, hit, result);
                    await NotifyBigRemaining(room, connectionId);
                    await NotifyPiercerRemaining(room, connectionId);
                    await NotifySlasherRemaining(room, connectionId);
                    await NotifyCrossRemaining(room, connectionId);

                    if (isGameOver)
                    {
                        await Clients.Group(room.RoomId).SendAsync("ReceiveGameOver", $"{room.Game.GetTeamByPlayer(connectionId)} wins!");
                        room.Game.GameOver = true;
                        break;
                    }
                }

                if (!isGameOver)
                {
                    room.Game.UpdateTurn();
                }
            }
        }

        public async Task NotifyBigRemaining(GameRoom room, string connectionId)
        {
            var team = room.Game.GetTeamByPlayer(connectionId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new BigShot().GetType());
            var teammates = room.Game.GetTeammates(connectionId);
            var player = room.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            await Clients.Client(player.ConnectionId).SendAsync("ReceiveBigAmount", amount);
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceiveBigAmount", amount);
            }
        }

        public async Task NotifyPiercerRemaining(GameRoom room, string connectionId)
        {
            var team = room.Game.GetTeamByPlayer(connectionId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new PiercerShot().GetType());
            var teammates = room.Game.GetTeammates(connectionId);
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceivePiercerAmount", amount);
            }
        }

        public async Task NotifySlasherRemaining(GameRoom room, string connectionId)
        {
            var team = room.Game.GetTeamByPlayer(connectionId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new SlasherShot().GetType());
            var teammates = room.Game.GetTeammates(connectionId);
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceiveSlasherAmount", amount);
            }
        }

        public async Task NotifyCrossRemaining(GameRoom room, string connectionId)
        {
            var team = room.Game.GetTeamByPlayer(connectionId) == "Team A" ? room.Game.ATeam : room.Game.BTeam;
            int amount = team.RemainingShots(new CrossShot().GetType());
            var teammates = room.Game.GetTeammates(connectionId);
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceiveCrossAmount", amount);
            }
        }

        private async Task NotifyTeammatesOfShot(GameRoom room, string connectionId, (int, int) hit, string result)
        {
            var teammates = room.Game.GetTeammates(connectionId);
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceiveShootResult", hit.Item1, hit.Item2, result);
            }
        }

        private async Task NotifyOpponentsOfShot(GameRoom room, string connectionId, (int, int) hit, string result)
        {
            var opponentTeammates = room.Game.GetTeammates(connectionId) == room.Game.ATeam.Players ? room.Game.BTeam.Players : room.Game.ATeam.Players;
            foreach (var opponentTeammate in opponentTeammates)
            {
                await Clients.Client(opponentTeammate.ConnectionId).SendAsync("ReceiveTeamHitResult", hit.Item1, hit.Item2, result);
            }
        }

        public async Task ConfirmTeamStrategy(string strategy)
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);
            if (room == null) return;

            var team = room.Game.GetTeamByPlayer(connectionId);

            // Set the strategy for the team
            room.Game.SetTeamStrategy(team, strategy);

            var teammates = room.Game.GetTeammates(connectionId);

            // Notify teammates about the selected strategy
            foreach (var teammate in teammates)
            {
                await Clients.Client(teammate.ConnectionId).SendAsync("ReceiveTeamStrategy", strategy);
                await NotifyBigRemaining(room, connectionId);
                await NotifyPiercerRemaining(room, connectionId);
                await NotifySlasherRemaining(room, connectionId);
                await NotifyCrossRemaining(room, connectionId);
            }
        }

        public async Task PlayerReadyForBattle()
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);
            if (room == null) return;

            var player = room.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player != null)
            {
                player.IsReadyForBattle = true;

                // Check if all players are ready
                if (room.Players.All(p => p.IsReadyForBattle))
                {
                    // Initialize the game and teams
                    room.Game.StartGame();
                    
                    // Place ships using the selected strategies
                    room.Game.PlaceShips();
                    room.Game.CountShots();
                    

                    // Send initial board states to all players
                    foreach (var p in room.Players)
                    {
                        var team = room.Game.GetTeamByPlayer(p.ConnectionId);
                        var board = team == "Team A" ? room.Game.ATeamBoard : room.Game.BTeamBoard;
                        await Clients.Client(p.ConnectionId).SendAsync("ReceiveBoardInfo", board.GetSerializableGrid());
                    }

                    // Signal to clients that the battle has started
                    await Clients.Group(room.RoomId).SendAsync("StartBattle");
                    
                    // Start the game by notifying the first player
                    await Clients.Client(room.Game.CurrentPlayerId).SendAsync("YourTurn");
                }
            }
        }
    }
}
