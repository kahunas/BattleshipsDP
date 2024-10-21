using Microsoft.AspNetCore.SignalR;
using SharedLibrary;

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

        public GameRoom? GetRoomByPlayerId(string playerId)
        {
            return _rooms.FirstOrDefault(r => r.Players.Any(p => p.ConnectionId == playerId));
        }

        public GameRoom CreateRoomStandart(string roomName)
        {
            //var room = new GameRoom(roomName, "standart");
            //Rooms.Add(room);
            //RefreshRoomList();

            var roomId = Guid.NewGuid().ToString();
            var room = new GameRoom(roomId, roomName, "standart");
            _rooms.Add(room);
            return room;
        }

        //public void CreateRoomMedium(string roomName)
        //{
        //    var room = new Room(roomName, "medium");
        //    Rooms.Add(room);
        //    RefreshRoomList();
        //}

        //public void CreateRoomAdvanced(string roomName)
        //{
        //    var room = new Room(roomName, "advanced");
        //    Rooms.Add(room);
        //    RefreshRoomList();
        //}


        //public GameRoom CreateRoom(string name)
        //{
        //    var roomId = Guid.NewGuid().ToString();
        //    var room = new GameRoom(roomId, name);
        //    _rooms.Add(room);
        //    return room;
        //}

        public bool TryAddPlayerToRoom(string roomId, Player player)
        {
            var room = GetRoomById(roomId);
            return room != null && room.TryAddPlayer(player);
        }

        public void StartSGame(string roomId)
        {
            var room = GetRoomById(roomId);
            room?.Game.StartGame();
        }

    }
}
