using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Basic
{
    public class ConversationChooser : IConversationChooser
    {
        private readonly IWhatsAppRepository respository;
        private readonly IConsole console;


        public ConversationChooser(IWhatsAppRepository respository, IConsole console)
        {
            this.respository = respository;
            this.console = console;
        }


        public async Task<IEnumerable<Message>> AskAndLoad()
        {
            var files = this.respository.GetFiles();
            var selectedFile = this.console.AskForItem(files);
            return this.respository.GetMessages(selectedFile)
                                   .Where(m => m.Sender.IsActive)
                                   .OrderBy(m => m.Timestamp);
        }
    }
}