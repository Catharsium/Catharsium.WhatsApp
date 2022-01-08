using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Terminal.Data;
using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class UpdateUsersActionHandler : IActionHandler
{
    private readonly IActiveUsersRepository activeUsersRepository;
    private readonly IConversationsRepository whatsAppRespository;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IEqualityComparer<User> userEqualityComparer;
    private readonly IFileFactory fileFactory;
    private readonly IJsonFileWriter jsonFileWriter;

    public string FriendlyName => "Update users";


    public UpdateUsersActionHandler(
        IActiveUsersRepository activeUsersRepository,
        IConversationsRepository whatsAppRespository,
        IConversationUsersRepository conversationUsersRepository,
        IEqualityComparer<User> userEqualityComparer,
        IFileFactory fileFactory,
        IJsonFileWriter jsonFileWriter)
    {
        this.activeUsersRepository = activeUsersRepository;
        this.whatsAppRespository = whatsAppRespository;
        this.conversationUsersRepository = conversationUsersRepository;
        this.userEqualityComparer = userEqualityComparer;
        this.fileFactory = fileFactory;
        this.jsonFileWriter = jsonFileWriter;
    }


    public async Task Run()
    {
        var conversations = await this.whatsAppRespository.GetConversations();
        var allUsers = new List<User>();
        foreach (var conversation in conversations) {
            var conversationUsers = this.activeUsersRepository.GetFor(conversation.Name);
            var users = await this.conversationUsersRepository.Add(conversationUsers, conversation.Name);
            var remainingUsers = allUsers.Where(allU => !users.Contains(allU, this.userEqualityComparer)).ToList();
            allUsers = remainingUsers;
            allUsers.AddRange(users);
        }

        var file = this.fileFactory.CreateFile($"E:\\Cloud\\OneDrive\\Data\\WhatsApp\\Data\\AllUsers.json");
        if (file.Exists) {
            file.Delete();
        }
        this.jsonFileWriter.Write(allUsers, file.FullName);
    }
}