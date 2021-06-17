using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Repository;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Basic
{
    public class ConversationChooser : IConversationChooser
    {
        private readonly IConversationRepository respository;
        private readonly IConversationUsersRepository conversationUsersRepository;
        private readonly IConsole console;


        public ConversationChooser(IConversationRepository respository, IConversationUsersRepository conversationUsersRepository, IConsole console)
        {
            this.respository = respository;
            this.conversationUsersRepository = conversationUsersRepository;
            this.console = console;
        }


        public async Task<IEnumerable<Message>> AskAndLoad()
        {
            var conversations = await this.respository.GetConversations();
            var selectedConversation = this.console.AskForItem(conversations);
            var conversationUsers = await this.conversationUsersRepository.ReadFrom(selectedConversation.Name);
            return (await this.respository.GetMessages(selectedConversation, conversationUsers)).OrderBy(m => m.Timestamp);
        }
    }
}