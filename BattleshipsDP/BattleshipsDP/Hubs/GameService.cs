using Microsoft.AspNetCore.SignalR;
using SharedLibrary;
using BattleshipsDP.Hubs;

namespace BattleshipsDP.Hubs
{
    public class GameService
    {
        private readonly List<GameRoom> _rooms = new(); // Instance-level, not static
        private readonly IHubContext<GameHub> _hubContext;

        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IEnumerable<GameRoom> GetAllRooms() => _rooms;

        public GameRoom? GetRoomById(string roomId) => _rooms.FirstOrDefault(r => r.RoomId == roomId);

        public GameRoom CreateRoom(string name)
        {
            var roomId = Guid.NewGuid().ToString();
            var room = new GameRoom(roomId, name);
            _rooms.Add(room);
            return room;
        }

        public bool TryAddPlayerToRoom(string roomId, Player player)
        {
            var room = GetRoomById(roomId);
            return room != null && room.TryAddPlayer(player);
        }

        public void StartGame(string roomId)
        {
            var room = GetRoomById(roomId);
            room?.Game.Start();
        }



    }
}
