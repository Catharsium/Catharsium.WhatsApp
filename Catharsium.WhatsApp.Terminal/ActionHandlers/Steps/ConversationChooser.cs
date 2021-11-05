using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Repository;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
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
            this.console.WriteLine();
            var conversations = await this.respository.GetConversations();
            var selectedConversation = this.console.AskForItem(conversations);
            if (selectedConversation != null) {
                this.console.WriteLine($"Conversation:\t{selectedConversation}");
                conversations.Clear();
                conversations.Add(selectedConversation);
            }

            var result = new List<Message>();
            foreach (var conversation in conversations) {
                var conversationUsers = await this.conversationUsersRepository.Get(conversation.Name);
                result.AddRange(await this.messageParser.GetMessages(conversation, conversationUsers));
            }

            result = result.Distinct(new MessageEqualityComparer()).OrderBy(m => m.Timestamp).ToList();
            this.console.WriteLine($"Total Messages:\t{result.Count}");
            this.console.WriteLine($"Last update:\t{result.Last().Timestamp:dd-MM-yyyy (HH:mm)}");

            return result;
        }
    }
}