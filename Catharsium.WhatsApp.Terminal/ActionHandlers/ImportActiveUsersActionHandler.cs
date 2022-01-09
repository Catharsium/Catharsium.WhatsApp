using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class ImportActiveUsersActionHandler : IActionHandler
{
    private readonly IActiveUsersRepository activeUsersRepository;
    private readonly IConversationRepository conversationRepository;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IEqualityComparer<User> userEqualityComparer;

    public string FriendlyName => "Update users";


    public ImportActiveUsersActionHandler(
        IActiveUsersRepository activeUsersRepository,
        IConversationRepository conversationRepository,
        IConversationUsersRepository conversationUsersRepository,
        IEqualityComparer<User> userEqualityComparer)
    {
        this.activeUsersRepository = activeUsersRepository;
        this.conversationRepository = conversationRepository;
        this.conversationUsersRepository = conversationUsersRepository;
        this.userEqualityComparer = userEqualityComparer;
    }


    public async Task Run()
    {
        var conversations = await this.conversationRepository.GetList();
        var allUsers = new List<User>();
        foreach (var conversation in conversations) {
            var activeUsers = this.activeUsersRepository.GetFor(conversation);
            var users = await this.conversationUsersRepository.Add(activeUsers, conversation);
            var remainingUsers = allUsers.Where(allU => !users.Contains(allU, this.userEqualityComparer)).ToList();
            allUsers = remainingUsers;
            allUsers.AddRange(users);
        }

        await this.conversationUsersRepository.Save();
    }
}