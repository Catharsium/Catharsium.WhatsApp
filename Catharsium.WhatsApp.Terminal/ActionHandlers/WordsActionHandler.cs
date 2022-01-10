using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Interfaces;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

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
        var conversation = await this.conversationChooser.AskAndLoad();
        var text = conversation.Messages.ToList()[1].Text;
        this.messageAnalyzer.GetEmoticons(text);
    }
}