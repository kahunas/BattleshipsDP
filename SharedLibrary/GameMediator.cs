// SharedLibrary/GameMediator.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedLibrary
{
    public class GameMediator : IMediator
    {
        private readonly Dictionary<string, GameRoom> _gameRooms;

        public GameMediator()
        {
            _gameRooms = new Dictionary<string, GameRoom>();
        }

        public void RegisterGameRoom(GameRoom gameRoom)
        {
            if (!_gameRooms.ContainsKey(gameRoom.RoomId))
            {
                _gameRooms.Add(gameRoom.RoomId, gameRoom);
            }
        }

        public void AddPlayerToRoom(string roomId, Player player)
        {
            if (_gameRooms.ContainsKey(roomId))
            {
                var room = _gameRooms[roomId];
                if (room.TryAddPlayer(player))
                {
                    Console.WriteLine($"Player {player.Name} added to room {room.RoomName}.");
                }
                else
                {
                    Console.WriteLine($"Failed to add player {player.Name} to room {room.RoomName}.");
                }
            }
            else
            {
                Console.WriteLine($"Room with ID {roomId} not found.");
            }
        }

        public void StartGame(string roomId)
        {
            if (_gameRooms.ContainsKey(roomId))
            {
                var room = _gameRooms[roomId];
                if (room.Players.Count >= 2)
                {
                    room.Game.StartGame();
                    Console.WriteLine($"Game started in room {room.RoomName}.");
                }
                else
                {
                    Console.WriteLine($"Not enough players to start the game in room {room.RoomName}.");
                }
            }
            else
            {
                Console.WriteLine($"Room with ID {roomId} not found.");
            }
        }

        public void NotifyPlayerReady(string roomId, string playerId)
        {
            if (_gameRooms.ContainsKey(roomId))
            {
                var room = _gameRooms[roomId];
                var player = room.Players.FirstOrDefault(p => p.ConnectionId == playerId);
                if (player != null)
                {
                    player.IsReadyForBattle = true;
                    Console.WriteLine($"Player {player.Name} is ready for battle in room {room.RoomName}.");
                }
            }
        }
    }
}