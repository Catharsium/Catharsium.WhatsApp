using Catharsium.Util.IO.Files.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Data.Repositories;

public class ConversationsRepository : IConversationsRepository
{
    private readonly IFileFactory fileFactory;
    private readonly IJsonFileReader jsonFileReader;
    private readonly IJsonFileWriter jsonFileWriter;
    private readonly WhatsAppDataSettings settings;


    public ConversationsRepository(IFileFactory fileFactory, IJsonFileReader jsonFileReader, IJsonFileWriter jsonFileWriter, WhatsAppDataSettings settings)
    {
        this.fileFactory = fileFactory;
        this.jsonFileReader = jsonFileReader;
        this.jsonFileWriter = jsonFileWriter;
        this.settings = settings;
    }


    public Task<List<string>> GetList()
    {
        var folder = this.fileFactory.CreateDirectory(this.settings.ConversationsFolder);
        return Task.Run(() => {
            if (!folder.Exists) {
                folder.Create();
            }

            return folder.GetFiles("*.json")
                         .Select(f => f.ExtensionlessName)
                         .ToList();
        });
    }


    public async Task<Conversation> Get(string name)
    {
        var file = this.fileFactory.CreateFile($"{this.settings.ConversationsFolder}\\{name}.json");
        return !file.Exists
            ? null
            : await Task.Run(() => this.jsonFileReader.ReadFrom<Conversation>(file)
        );
    }


    public Task Save(Conversation conversation)
    {
        var file = this.fileFactory.CreateFile($"{this.settings.ConversationsFolder}\\{conversation.Name}.json");
        return Task.Run(() => {
            if (file.Exists) {
                file.Delete();
            }

            this.jsonFileWriter.Write(conversation, file);
        });
    }
}