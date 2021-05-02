using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Ui.Terminal.ActionHandlers
{
    public class UsersActionHandler : IActionHandler
    {
        private readonly IWhatsAppExportFile file;
        private readonly IConsole console;

        public string FriendlyName => "Users";


        public UsersActionHandler(IWhatsAppExportFile file, IConsole console)
        {
            this.file = file;
            this.console = console;
        }


        public async Task Run()
        {
            var messages = this.file.GetMessages();
            this.console.WriteLine(messages.Count().ToString());
            var users = messages.Select(m => m.Sender).Distinct().OrderBy(u => u);
            this.console.WriteLine($"{users.Count()}");
            foreach (var user in users) {
                this.console.WriteLine($"{user}");
            }
        }
    }
}