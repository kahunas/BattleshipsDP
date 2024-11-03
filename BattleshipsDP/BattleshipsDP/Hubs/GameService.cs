using BattleshipsDP.Client.Pages;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary;

namespace BattleshipsDP.Hubs
{
    public class GameService
    {
        private readonly List<GameRoom> _rooms = new(); // Instance-level, not static
        private readonly IHubContext<GameHub> _hubContext;
        private readonly object _lock = new object();

        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IEnumerable<GameRoom> GetAllRooms()
        {
            lock (_lock)
            {
                return _rooms.ToList();
            }
        }

        public GameRoom GetRoomById(string roomId)
        {
            lock (_lock)
            {
                return _rooms.FirstOrDefault(r => r.RoomId == roomId);
            }
        }

        public GameRoom? GetRoomByPlayerId(string playerId)
        {
            lock (_lock)
            {
                return _rooms.FirstOrDefault(r => r.Players.Any(p => p.ConnectionId == playerId));
            }
        }

        public GameRoom CreateRoom(string name, string difficulty)
        {
            lock (_lock)
            {
                var roomId = Guid.NewGuid().ToString();
                var room = new GameRoom(roomId, name, difficulty);  // Pass the difficulty level
                _rooms.Add(room);
                return room;
            }
        }

        public bool TryAddPlayerToRoom(string roomId, Player player)
        {
            lock (_lock)
            {
                var room = GetRoomById(roomId);
                return room != null && room.TryAddPlayer(player);
            }
        }

        public void StartGame(string roomId)
        {
            var room = GetRoomById(roomId);
            room?.Game.StartGame();
        }

    }
}
