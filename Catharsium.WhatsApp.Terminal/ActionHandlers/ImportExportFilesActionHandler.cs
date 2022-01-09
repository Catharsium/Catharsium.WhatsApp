using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class ImportExportFilesActionHandler : IActionHandler
{
    private readonly IExportFilesRepository exportFilesRepository;
    private readonly IConversationRepository conversationRepository;

    public string FriendlyName => "Import export files";


    public ImportExportFilesActionHandler(IExportFilesRepository exportFilesRepository, IConversationRepository conversationRepository)
    {
        this.exportFilesRepository = exportFilesRepository;
        this.conversationRepository = conversationRepository;
    }


    public async Task Run()
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