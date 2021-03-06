using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.WhatsApp.Terminal._Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.WhatsApp.Terminal;

class Program
{
    static async Task Main(string[] args)
    {
        var appsettingsFilePath = @"E:\Cloud\OneDrive\Software\Catharsium.WhatsApp\appsettings.json";
        //var appsettingsFilePath = @"C:\Users\twbra\OneDrive\Software\Catharsium.WhatsApp\appsettings.json";
        if (args.Length > 0) {
            appsettingsFilePath = args[0];
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appsettingsFilePath, false, false);
        var configuration = builder.Build();

        var serviceProvider = new ServiceCollection()
            .AddWhatsAppTerminal(configuration)
            .BuildServiceProvider();

        var mainMenuActionHandler = serviceProvider.GetService<ISingleMenuActionHandler>();
        await mainMenuActionHandler.Run();
    }
}