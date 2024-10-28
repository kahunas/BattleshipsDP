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
                    await Clients.Client(player.ConnectionId).SendAsync("ReceivePlayerInfo", player.Name, player.ConnectionId, team);
                }
            }
        }

        public async Task RequestBoard()
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);

            if (room == null) return;

            // Determine which board to send based on the team
            var team = room.Game.GetTeamByPlayer(connectionId);
            var board = team == "Team A" ? room.Game.ATeam.Board : room.Game.BTeam.Board;

            // Get the serializable version of the board
            var serializableBoard = board.GetSerializableGrid();

            // Send the board to the client
            await Clients.Client(connectionId).SendAsync("ReceiveBoardState", serializableBoard);
        }

        public async Task PlayerReady()
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);

            if (room == null) return;

            var player = room.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null) return;

            room.Game.SetPlayerReady(connectionId);

            // Register player as a turn observer
            room.Game.RegisterTurnObserver(new TurnObserver(player.ConnectionId, Clients));

            var team = room.Game.GetTeamByPlayer(connectionId);
            var board = team == "Team A" ? room.Game.ATeam.Board : room.Game.BTeam.Board;

            // Use the player object to send the name and other info to the client
            await Clients.Client(connectionId).SendAsync("ReceivePlayerInfo", player.Name, connectionId, team);

            // Convert the board to a serializable format and send it
            var serializableBoard = board.GetSerializableGrid();
            await Clients.Client(connectionId).SendAsync("ReceiveBoardInfo", serializableBoard);
        }

        public async Task HighlightBlockForTeam(int row, int col)
        {
            var connectionId = Context.ConnectionId;
            var room = _gameService.GetRoomByPlayerId(connectionId);

            if (room == null) return;

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
            var shots = room.Game.DefineShots();

            if (room == null) return;
            if (room.Game.CurrentPlayerId != connectionId)
            {
                await Clients.Client(connectionId).SendAsync("NotYourTurn");
                return;
            }

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
    }
}
