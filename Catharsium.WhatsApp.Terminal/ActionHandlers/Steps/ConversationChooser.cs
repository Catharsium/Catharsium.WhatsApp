using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Steps;

public class ConversationChooser : ISelectionActionStep<Conversation>
{
    private readonly IConversationsRepository conversationRepository;
    private readonly IConsole console;


    public ConversationChooser(IConversationsRepository conversationRepository, IConsole console)
    {
        this.conversationRepository = conversationRepository;
        this.console = console;
    }


    public async Task<Conversation> Select()
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