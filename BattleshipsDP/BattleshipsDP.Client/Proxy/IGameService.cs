using Microsoft.AspNetCore.Components;
using SharedLibrary;
using BattleshipsDP.Client;
using Microsoft.AspNetCore.SignalR.Client;
using BattleshipsDP.Client.Pages;

namespace BattleshipsDP.Client.Proxy
{
    public interface IRoomService
    {
        Task CreateRoom(string roomName, string playerName, string difficulty);
        Task JoinRoom(string roomId, string playerName, List<GameRoom> rooms);
    }

    public class RoomService : IRoomService
    {
        private readonly PlayerState _playerState;
        private readonly NavigationManager _navigationManager;

        public RoomService(PlayerState playerState, NavigationManager navigationManager)
        {
            _playerState = playerState;
            _navigationManager = navigationManager;
        }

        public async Task CreateRoom(string roomName, string playerName, string difficulty)
        {
            GameRoom room = await PlayerState.HubConnection.InvokeAsync<GameRoom>(
                "CreateRoom", roomName, playerName, difficulty);

            if (room != null)
            {
                _navigationManager.NavigateTo($"/room/{room.RoomId}");
            }
        }

        public async Task JoinRoom(string roomId, string playerName, List<GameRoom> rooms)
        {
            var room = await PlayerState.HubConnection.InvokeAsync<GameRoom>(
                "JoinRoom", roomId, playerName);

            if (room != null)
            {
                var roomToUpdate = rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (roomToUpdate != null)
                {
                    roomToUpdate.Players = room.Players;
                }
                _navigationManager.NavigateTo($"/room/{room.RoomId}");
            }
            else
            {
                Console.WriteLine("Room is full or does not exist.");
            }
        }
    }
}
