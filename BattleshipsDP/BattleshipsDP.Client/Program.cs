using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharedLibrary;
using SharedLibrary.Visitor;

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
