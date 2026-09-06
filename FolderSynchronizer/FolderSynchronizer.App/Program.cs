using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FolderSynchronizer.App
{
    internal class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var options = CommandLineArgumentsParser.Parse(args);

                var builder = Host.CreateApplicationBuilder();

                builder.Services.AddSingleton(options);
                builder.Services.AddSingleton<FolderSynchronizationService>();
                builder.Services.AddHostedService<FolderSynchronizationBackgroundService>();

                using var host = builder.Build();
                host.Run();

                return 0;
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Invalid arguments: {ex.Message}");
                return 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Synchronization failed: {ex.Message}");
                return 1;
            }
        }
    }
}
