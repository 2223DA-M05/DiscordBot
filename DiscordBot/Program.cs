using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureHostConfiguration(ConfigureConfiguration)
                .ConfigureServices(ConfigureServices)
                .RunConsoleAsync();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<DiscordBotService>();
        }

        static void ConfigureConfiguration(IConfigurationBuilder builder)
        {
            builder.AddUserSecrets<Program>();
        }
    }
}