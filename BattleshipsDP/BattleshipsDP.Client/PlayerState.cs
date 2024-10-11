using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SharedLibrary;

namespace BattleshipsDP.Client
{
    public class PlayerState
    {
        public Player CurrentPlayer { get; private set; }
        public static HubConnection? HubConnection { get; private set; }
        public event Action OnChange;

        public void SetUser(Player player)
        {
            CurrentPlayer = player;
            NotifyStateChanged();
        }

        // Initialize and manage the HubConnection
        public async Task InitializeHubConnection(string hubUrl, NavigationManager navigationManager)
        {
            if (HubConnection == null)
            {
                HubConnection = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri(hubUrl))
                    .WithAutomaticReconnect() // Optional: Handle reconnections
                    .Build();

                await HubConnection.StartAsync();
                NotifyStateChanged();
            }
        }

        // Method to dispose/stop the connection
        public async Task StopHubConnection()
        {
            if (HubConnection != null)
            {
                await HubConnection.StopAsync();
                await HubConnection.DisposeAsync();
                HubConnection = null;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
