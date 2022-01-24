﻿using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class UserStatisticsActionHandler : IActionHandler
{
    private readonly IConversationsRepository conversationRepository;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IMessageParser messageParser;
    private readonly IEqualityComparer<User> userEqualityComparer;
    private readonly IConsole console;

    public string DisplayName => "User Statistics";


    public UserStatisticsActionHandler(
        IConversationsRepository conversationRepository,
        IConversationUsersRepository conversationUsersRepository,
        IMessageParser messageParser,
        IEqualityComparer<User> userEqualityComparer,
        IConsole console)
    {
        this.conversationRepository = conversationRepository;
        this.conversationUsersRepository = conversationUsersRepository;
        this.messageParser = messageParser;
        this.userEqualityComparer = userEqualityComparer;
        this.console = console;
    }


    public async Task Run()
    {
        //var conversations = await this.conversationRepository.GetList();
        //foreach (var conversationName in conversations) {
        //    var conversationUsers = await this.conversationUsersRepository.Get(conversationName);
        //    var conversation = this.conversationRepository.Get(conversationName);
        //}

        //var allMessages = conversations.SelectMany(c => c.Messages);
        //var allUsers = allMessages.Select(m => m.Sender).Distinct(this.userEqualityComparer).OrderBy(u => u.DisplayName);
        //var selectedUser = this.console.AskForItem(allUsers, "Select a user");
        //var messages = allMessages.Include(new UserFilter(selectedUser)).ToList();

        //this.console.WriteLine($"Total messages: {messages.Count}");
    }
}