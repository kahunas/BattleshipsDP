using BattleshipsDP.Client;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary;
using System.Numerics;

namespace BattleshipsDP.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _gameService;
        public GameHub(GameService gameService)
        {
            _gameService = gameService;
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

        public async Task<GameRoom> CreateRoom(string name, string playerName)
        {
            GameRoom room = _gameService.CreateRoom(name);
            var newPlayer = new Player(Context.ConnectionId, playerName);
            _gameService.TryAddPlayerToRoom(room.RoomId, newPlayer);
            //room.TryAddPlayer(newPlayer);

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

            if (room == null || !room.Game.GameStarted || room.Game.GameOver) return;

            // Early validation of turn
            if (room.Game.CurrentPlayerId != connectionId)
            {
                await Clients.Client(connectionId).SendAsync("NotYourTurn");
                return;
            }

            var shots = room.Game.DefineShots();

            var shot = shots.FirstOrDefault(s => s.Name.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (shot == null)
            {
                await Clients.Client(connectionId).SendAsync("InvalidShotType");
                return;
            }

            var coordinates = shot.ShotCoordinates(row, col);
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
