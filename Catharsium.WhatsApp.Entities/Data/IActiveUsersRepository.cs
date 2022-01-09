﻿using Catharsium.WhatsApp.Entities.Models;

namespace Catharsium.WhatsApp.Entities.Data;

public interface IActiveUsersRepository
{
    List<User> GetFor(string conversationName);
}