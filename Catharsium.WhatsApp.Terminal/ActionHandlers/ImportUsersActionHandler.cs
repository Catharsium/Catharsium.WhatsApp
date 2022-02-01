using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class ImportUsersActionHandler : BaseActionHandler
{
    private readonly IExportUsersRepository activeUsersRepository;
    private readonly IConversationsRepository conversationRepository;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IEqualityComparer<User> userEqualityComparer;


    public ImportUsersActionHandler(
        IExportUsersRepository activeUsersRepository,
        IConversationsRepository conversationRepository,
        IConversationUsersRepository conversationUsersRepository,
        IEqualityComparer<User> userEqualityComparer,
        IConsole console)
        : base(console, "Import users")
    {
        this.activeUsersRepository = activeUsersRepository;
        this.conversationRepository = conversationRepository;
        this.conversationUsersRepository = conversationUsersRepository;
        this.userEqualityComparer = userEqualityComparer;
    }


    public override async Task Run()
    {
        var conversations = await this.conversationRepository.GetList();
        var allUsers = new List<User>();
        foreach (var conversation in conversations) {
            var activeUsers = this.activeUsersRepository.GetForConversation(conversation);
            var users = await this.conversationUsersRepository.Add(activeUsers, conversation);
            var remainingUsers = allUsers.Where(allU => !users.Contains(allU, this.userEqualityComparer)).ToList();
            allUsers = remainingUsers;
            allUsers.AddRange(users);
        }

        await this.conversationUsersRepository.Save();
    }
}