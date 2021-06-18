using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Data.Repository;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class UserStatisticsActionHandler : IActionHandler
    {
        private readonly IConversationsRepository conversationsRepository;
        private readonly IConversationUsersRepository conversationUsersRepository;
        private readonly IMessageParser messageParser;
        private readonly IEqualityComparer<User> userEqualityComparer;
        private readonly IConsole console;

        public string FriendlyName => "User Statistics";


        public UserStatisticsActionHandler(
            IConversationsRepository conversationsRepository, 
            IConversationUsersRepository conversationUsersRepository, 
            IMessageParser messageParser, 
            IEqualityComparer<User> userEqualityComparer, 
            IConsole console)
        {
            this.conversationsRepository = conversationsRepository;
            this.conversationUsersRepository = conversationUsersRepository;
            this.messageParser = messageParser;
            this.userEqualityComparer = userEqualityComparer;
            this.console = console;
        }


        public async Task Run()
        {
            var conversations = await this.conversationsRepository.GetConversations();
            foreach (var conversation in conversations) {
                var conversationUsers = await this.conversationUsersRepository.GetAll(conversation.Name);
                conversation.Messages = (await this.messageParser.GetMessages(conversation, conversationUsers)).OrderBy(m => m.Timestamp).ToList();
            }

            var allMessages = conversations.SelectMany(c => c.Messages);
            var allUsers = allMessages.Select(m => m.Sender).Distinct(this.userEqualityComparer).OrderBy(u => u.DisplayName);
            var selectedUser = this.console.AskForItem(allUsers, "Select a user");
            var messages = allMessages.Include(new UserFilter(selectedUser)).ToList();

            this.console.WriteLine($"Total messages: {messages.Count}");
        }
    }
}