using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class ActionHandler : IActionHandler
    {
        private readonly IConversationChooser conversationChooser;
        private readonly IPeriodChooser periodChooser;
        private readonly IConsole console;

        public string FriendlyName => "test";


        public ActionHandler(IConversationChooser conversationChooser, IPeriodChooser periodChooser, IConsole console)
        {
            this.conversationChooser = conversationChooser;
            this.periodChooser = periodChooser;
            this.console = console;
        }


        public async Task Run()
        {
            var messages = await this.conversationChooser.AskAndLoad();
            var users = messages.Select(m => m.Sender).Distinct(new UserEqualityComparer()).OrderBy(u => messages.Where(m => m.Sender == u).Count()).ToList();
            var maxName = users.Max(u => u.ToString().Length);
            this.console.WriteLine("Sexyness Per User");
            foreach (var user in users) {
                if (user.DisplayName == "Bart") {
                    continue;
                }
                this.console.Write($"[1]  ");
                this.console.Write(user.ToString());
                this.console.FillBlock(user.ToString().Length, maxName + 5);
                this.console.WriteLine("100%");
            }

            this.console.Write($"[{users.Count}] ");
            this.console.Write("Bart");
            this.console.FillBlock("Bart".Length, maxName + 5);
            this.console.WriteLine("  0%");
        }
    }
}