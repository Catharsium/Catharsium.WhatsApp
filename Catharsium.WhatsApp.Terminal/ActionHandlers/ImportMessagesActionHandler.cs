using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class ImportMessagesActionHandler : BaseActionHandler
{
    private readonly IExportFilesRepository exportFilesRepository;
    private readonly IConversationsRepository conversationRepository;



    public ImportMessagesActionHandler(IExportFilesRepository exportFilesRepository, IConversationsRepository conversationRepository, IConsole console)
        : base(console, "Import messages")
    {
        this.exportFilesRepository = exportFilesRepository;
        this.conversationRepository = conversationRepository;
    }


    public override async Task Run()
    {
        var conversations = await this.exportFilesRepository.GetConversations();
        foreach (var conversation in conversations) {
            var existingConversation = await this.conversationRepository.Get(conversation.Name);
            if (existingConversation == null) {
                existingConversation = new Conversation { Name = conversation.Name };
            }

            existingConversation.Messages.AddRange(conversation.Messages);
            existingConversation.Messages = existingConversation.Messages.Distinct(new MessageEqualityComparer()).ToList();
            await this.conversationRepository.Save(existingConversation);
        }
    }
}