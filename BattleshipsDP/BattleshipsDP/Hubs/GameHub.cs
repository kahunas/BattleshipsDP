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

        private static readonly List<GameRoom> _rooms = new();
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Player with ID {Context.ConnectionId} connected.");

            await Clients.Caller.SendAsync("Rooms", _rooms.OrderBy(r => r.RoomName));

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Player with ID {Context.ConnectionId} disconnected.");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<GameRoom> CreateRoom(string name, string playerName)
        {
            var roomId = Guid.NewGuid().ToString();
            GameRoom room = new GameRoom(roomId, name);
            _rooms.Add(room);

            Player newPlayer = new Player(Context.ConnectionId.ToString(), playerName);
            room.TryAddPlayer(newPlayer);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.All.SendAsync("Rooms", _rooms.OrderBy(r => r.RoomName));

            return room;
        }

        public async Task<GameRoom?> JoinRoom(string roomId, string playerName)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room is not null)
            {
                var newPlayer = new Player(Context.ConnectionId, playerName);
                if (room.TryAddPlayer(newPlayer))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                    await Clients.Group(roomId).SendAsync("PlayerJoined", newPlayer);
                    return room;
                }
            }

            return null;
        }

        public async Task<GameRoom> GetRoomById(string roomId)
        {
            return _gameService.GetRoomById(roomId);

        }

        public async Task StartGame(string roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
            if(room is not null)
            {
                //room.AssignPlayersToTeams();
                room.Game.Start();
                //await Clients.Group(roomId).SendAsync("UpdateGame", room);
                await Clients.Group(roomId).SendAsync("NavigateToBattle");
            }
        }
    }
}
