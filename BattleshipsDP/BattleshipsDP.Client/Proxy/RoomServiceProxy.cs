using SharedLibrary;
namespace BattleshipsDP.Client.Proxy
{
    public class RoomServiceProxy : IRoomService
    {
        private readonly RoomService _realService;

        public RoomServiceProxy(RoomService realService)
        {
            _realService = realService;
        }

        public async Task CreateRoom(string roomName, string playerName, string difficulty)
        {
            // Validation logic
            if (string.IsNullOrEmpty(playerName))
            {
                Console.WriteLine("Player name is required.");
                return;
            }

            if (string.IsNullOrEmpty(roomName))
            {
                Console.WriteLine("Room name cannot be empty.");
                return;
            }

            if (string.IsNullOrEmpty(difficulty))
            {
                Console.WriteLine("Please select a difficulty.");
                return;
            }

            // Delegate to the real service
            await _realService.CreateRoom(roomName, playerName, difficulty);
        }

        public async Task JoinRoom(string roomId, string playerName, List<GameRoom> rooms)
        {
            // Validation logic
            if (string.IsNullOrEmpty(playerName))
            {
                Console.WriteLine("Player name is required.");
                return;
            }

            if (string.IsNullOrEmpty(roomId))
            {
                Console.WriteLine("Room ID cannot be empty.");
                return;
            }

            // Delegate to the real service
            await _realService.JoinRoom(roomId, playerName, rooms);
        }
    }
}
