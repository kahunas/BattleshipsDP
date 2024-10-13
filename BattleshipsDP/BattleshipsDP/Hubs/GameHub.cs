using BattleshipsDP.Client.Pages;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary;

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
            _gameService.StartGame(roomId);
            await Clients.Group(roomId).SendAsync("NavigateToBattle");
        }

    }
}
