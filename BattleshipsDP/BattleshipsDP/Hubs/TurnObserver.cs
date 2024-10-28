using Microsoft.AspNetCore.SignalR;
using SharedLibrary;

namespace BattleshipsDP.Hubs
{
    public class TurnObserver : ITurnObserver
    {
        private readonly string _connectionId;
        private readonly IHubCallerClients _clients;

        public TurnObserver(string connectionId, IHubCallerClients clients)
        {
            _connectionId = connectionId;
            _clients = clients;
        }

        public async void UpdateTurn(string currentPlayer)
        {
            if (_connectionId == currentPlayer)
            {
                await _clients.Client(_connectionId).SendAsync("YourTurn");
            }
            Console.WriteLine($"Notifying {_connectionId} of their turn."); // Test
        }
    }
}
