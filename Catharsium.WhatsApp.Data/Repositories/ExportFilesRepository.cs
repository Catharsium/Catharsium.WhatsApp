using Catharsium.Util.IO.Files.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Data.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using System.Text.RegularExpressions;
namespace Catharsium.WhatsApp.Data.Repositories;

public class ExportFilesRepository : IExportFilesRepository
{
    private readonly IFileFactory fileFactory;
    private readonly IMessageParser messageParser;
    private readonly IExportUsersRepository activeUsersRepository;
    private readonly WhatsAppDataSettings settings;


    public ExportFilesRepository(IFileFactory fileFactory, IMessageParser messageParser, IExportUsersRepository activeUsersRepository, WhatsAppDataSettings settings)
    {
        this.fileFactory = fileFactory;
        this.messageParser = messageParser;
        this.activeUsersRepository = activeUsersRepository;
        this.settings = settings;
    }


    private Task<IFile[]> GetFiles()
    {
        return Task.FromResult(this.fileFactory.CreateDirectory(this.settings.ExportFilesFolder).GetFiles("*.txt"));
    }


    public async Task<List<Conversation>> GetConversations()
    {
        var result = new List<Conversation>();
        var files = await this.GetFiles();

        foreach (var file in files) {
            var name = this.GetConversationName(file);
            var conversation = result.FirstOrDefault(c => c.Name == name);
            if (conversation == null) {
                conversation = new Conversation {
                    Name = name,
                    Messages = new List<Message>()
                };
                result.Add(conversation);
            }

            conversation.Messages.AddRange(await this.messageParser.GetMessages(file));
            conversation.Messages = conversation.Messages.Distinct(new MessageEqualityComparer()).ToList();
        }

        return result;
    }


    private string GetConversationName(IFile file)
    {
        var regex = new Regex("^(.+) (\\d+)$");
        var match = regex.Match(file.ExtensionlessName);
        var name = file.ExtensionlessName;
        if (match.Success && match.Groups.Count == 3) {
            name = match.Groups[1].Value;
        }

        return name;
    }
}