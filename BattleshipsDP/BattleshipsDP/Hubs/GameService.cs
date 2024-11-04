using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipsDP.Hubs
{
    public class GameService
    {
        // Private static instance of the singleton
        private static GameService _instance;
        private static readonly object _lock = new object();

        // Instance-level fields
        private readonly List<GameRoom> _rooms = new();

        // Private constructor to prevent instantiation from outside
        private GameService() { }

        // Public static method to get or create the singleton instance
        public static GameService Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new GameService();
                }
            }
        }

        public IEnumerable<GameRoom> GetAllRooms()
        {
            return _rooms.ToList();
        }

        public GameRoom GetRoomById(string roomId)
        {
            return _rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        public GameRoom? GetRoomByPlayerId(string playerId)
        {
            return _rooms.FirstOrDefault(r => r.Players.Any(p => p.ConnectionId == playerId));
        }

        public GameRoom CreateRoom(string name, string difficulty)
        {
            var roomId = Guid.NewGuid().ToString();
            var room = new GameRoom(roomId, name, difficulty);
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
            room?.Game.StartGame();
        }
    }

}


