using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharedLibrary;

namespace BattleshipsDP.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddScoped<PlayerState>();

            await builder.Build().RunAsync();
        }
    }
}
