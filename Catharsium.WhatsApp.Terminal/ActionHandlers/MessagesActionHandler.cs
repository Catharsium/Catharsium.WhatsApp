using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class MessagesActionHandler : IActionHandler
    {
        private readonly IConversationChooser conversationChooser;
        private readonly IPeriodChooser periodChooser;
        private readonly IEqualityComparer<User> userEqualityComparer;
        private readonly IConsole console;

        public string FriendlyName => "Messages";


        public MessagesActionHandler(IConversationChooser conversationChooser, IPeriodChooser periodChooser, IEqualityComparer<User> userEqualityComparer, IConsole console)
        {
            this.conversationChooser = conversationChooser;
            this.periodChooser = periodChooser;
            this.userEqualityComparer = userEqualityComparer;
            this.console = console;
        }


        public async Task Run()
        {
            var period = await this.periodChooser.AskForPeriod();
            var messages = await this.conversationChooser.AskAndLoad();
            messages = messages.Include(new PeriodFilter(period));
            var users = messages.Select(m => m.Sender).Distinct(this.userEqualityComparer);
            var user = this.console.AskForItem(users);
            if (user != null) {
                messages = messages.Include(new UserFilter(user));
            }

            foreach(var message in messages) {
                this.console.WriteLine($"{message}");
            }
        }
    }
}