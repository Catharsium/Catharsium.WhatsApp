using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Steps;

public class ConversationChooser : IConversationChooser
{
    private readonly IConversationRepository conversationRepository;
    private readonly IConsole console;


    public ConversationChooser(IConversationRepository conversationRepository, IConsole console)
    {
        this.conversationRepository = conversationRepository;
        this.console = console;
    }


    public async Task<Conversation> AskAndLoad()
    {
        this.console.WriteLine();
        var conversations = await this.conversationRepository.GetList();
        var selectedConversation = this.console.AskForItem(conversations);
        if (selectedConversation != null) {
            this.console.WriteLine($"Conversation:\t{selectedConversation}");
            conversations.Clear();
            conversations.Add(selectedConversation);
        }

        var result = await this.conversationRepository.Get(selectedConversation);
        this.console.WriteLine($"Total Messages:\t{result.Messages.Count}");
        this.console.WriteLine($"Last update:\t{result.Messages.Max(m => m.Timestamp):dd-MM-yyyy (HH:mm)}");

        return result;
    }
}