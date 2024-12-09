using Microsoft.AspNetCore.SignalR.Client;
using SharedLibrary;
using Microsoft.AspNetCore.Components;

namespace BattleshipsDP.Client.Facade
{
    public class PlayerStateFacade
    {
        private readonly PlayerState _playerState;

        public PlayerStateFacade(PlayerState playerState)
        {
            _playerState = playerState;
        }

        public Player CurrentPlayer => _playerState.CurrentPlayer;

        public HubConnection? HubConnection => PlayerState.HubConnection;

        public event Action OnChange
        {
            add => _playerState.OnChange += value;
            remove => _playerState.OnChange -= value;
        }

        public void SetUser(Player player) => _playerState.SetUser(player);

        public async Task InitializeHubConnection(string hubUrl, NavigationManager navigationManager)
        {
            await _playerState.InitializeHubConnection(hubUrl, navigationManager);
        }

        public async Task StopHubConnection() => await _playerState.StopHubConnection();

        public bool IsHubConnectionActive => PlayerState.HubConnection != null && PlayerState.HubConnection.State == HubConnectionState.Connected;

        public async Task<T> InvokeAsync<T>(string methodName, params object[] args)
        {
            if (HubConnection == null) throw new InvalidOperationException("HubConnection is not initialized.");
            return await HubConnection.InvokeAsync<T>(methodName, args);
        }

        public async Task SendAsync(string methodName, params object[] args)
        {
            if (HubConnection == null) throw new InvalidOperationException("HubConnection is not initialized.");
            await HubConnection.SendAsync(methodName, args);
        }

        // Method for subscribing to hub events with single or multiple arguments
        public void On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> handler)
        {
            HubConnection?.On(methodName, handler);
        }

        public void On<T1, T2, T3>(string methodName, Action<T1, T2, T3> handler)
        {
            HubConnection?.On(methodName, handler);
        }

        public void On<T1, T2>(string methodName, Action<T1, T2> handler)
        {
            HubConnection?.On(methodName, handler);
        }

        public void On<T>(string methodName, Action<T> handler)
        {
            HubConnection?.On(methodName, handler);
        }

        public void On(string methodName, Action handler)
        {
            HubConnection?.On(methodName, handler);
        }
    }
}