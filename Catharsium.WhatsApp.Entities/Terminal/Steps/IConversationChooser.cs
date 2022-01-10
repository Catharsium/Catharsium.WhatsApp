﻿using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Terminal.Steps;

public interface IConversationChooser
{
    Task<Conversation> AskForConversation();
}