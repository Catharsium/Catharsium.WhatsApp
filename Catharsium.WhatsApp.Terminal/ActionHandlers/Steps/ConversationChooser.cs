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
        private readonly IConversationsRepository respository;
        private readonly IMessageParser messageParser;
        private readonly IConversationUsersRepository conversationUsersRepository;
        private readonly IConsole console;


        public ConversationChooser(IConversationsRepository respository, IMessageParser messageParser, IConversationUsersRepository conversationUsersRepository, IConsole console)
        {
            this.respository = respository;
            this.messageParser = messageParser;
            this.conversationUsersRepository = conversationUsersRepository;
            this.console = console;
        }


        public async Task<IEnumerable<Message>> AskAndLoad()
        {
            var conversations = await this.respository.GetConversations();
            var selectedConversation = this.console.AskForItem(conversations);
            var conversationUsers = await this.conversationUsersRepository.GetAll(selectedConversation.Name);
            return (await this.messageParser.GetMessages(selectedConversation, conversationUsers)).OrderBy(m => m.Timestamp);
        }
    }
}