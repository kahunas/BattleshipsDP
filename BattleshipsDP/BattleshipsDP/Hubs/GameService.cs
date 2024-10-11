using Microsoft.AspNetCore.SignalR;
using SharedLibrary;
using BattleshipsDP.Hubs;

namespace BattleshipsDP.Hubs
{
    public class GameService
    {
        private static readonly List<GameRoom> _rooms = new();
        private readonly IHubContext<GameHub> _hubContext;

        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IEnumerable<GameRoom> GetAllRooms() => _rooms;
        public GameRoom GetRoomById(string roomId) => _rooms.FirstOrDefault(r => r.RoomId == roomId);



    }
}
