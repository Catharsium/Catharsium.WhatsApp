using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Terminal.Steps;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Steps
{
    public class UserChooser : IUserChooser
    {
        private readonly IConversationUsersRepository conversationUsersRepository;
        private readonly IConsole console;

        public UserChooser(IConversationUsersRepository conversationUsersRepository, IConsole console)
        {
            this.conversationUsersRepository = conversationUsersRepository;
            this.console = console;
        }


        public async Task<User> AskForUser(string conversationName)
        {
            var users = await this.conversationUsersRepository.Get(conversationName);
            return this.console.AskForItem(users);
        }
    }
}