using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class WordsActionHandler : IActionHandler
    {
        private readonly IConversationChooser conversationChooser;
        private readonly IMessageAnalyzer messageAnalyzer;


        public WordsActionHandler(IConversationChooser conversationChooser, IMessageAnalyzer messageAnalyzer)
        {
            this.conversationChooser = conversationChooser;
            this.messageAnalyzer = messageAnalyzer;
        }


        public string FriendlyName => "Words";


        public async Task Run()
        {
            var messages = await this.conversationChooser.AskAndLoad();
            var text = messages.ToList()[1].Text;
            this.messageAnalyzer.GetEmoticons(text);
        }
    }
}