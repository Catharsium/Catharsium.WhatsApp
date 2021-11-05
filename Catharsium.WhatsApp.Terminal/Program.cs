using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Ui.Terminal._Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Ui.Terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"E:\Cloud\OneDrive\Software\Catharsium.WhatsApp\appsettings.json", false, false);
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddWhatsAppTerminal(configuration)
                .BuildServiceProvider();

            var chooseOperationActionHandler = serviceProvider.GetService<IChooseActionHandler>();
            await chooseOperationActionHandler.Run();
        }
    }
}