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

            // Register Team A Visitor
            builder.Services.AddScoped<TeamAStatisticsVisitor>();

            // Register Team B Visitor
            builder.Services.AddScoped<TeamBStatisticsVisitor>();

            await builder.Build().RunAsync();
        }
    }
}
