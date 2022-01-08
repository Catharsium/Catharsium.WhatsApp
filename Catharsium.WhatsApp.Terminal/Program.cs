using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Terminal._Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catharsium.WhatsApp.Terminal;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"E:\Cloud\OneDrive\Software\Catharsium.WhatsApp\appsettings.json", false, false);
        var configuration = builder.Build();

        var serviceProvider = new ServiceCollection()
            .AddWhatsAppTerminal(configuration)
            .BuildServiceProvider();

        var chooseOperationActionHandler = serviceProvider.GetService<IChooseActionHandler>();
        await chooseOperationActionHandler.Run();
    }
}