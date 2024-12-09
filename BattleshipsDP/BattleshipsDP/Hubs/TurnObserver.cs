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

        public async Task UpdateTurn(string currentPlayer)
        {
            try
            {
                if (_connectionId == currentPlayer)
                {
                    await _clients.Client(_connectionId).SendAsync("YourTurn");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine($"Error notifying {_connectionId}: {ex.Message}");
            }
        }
    }
}
