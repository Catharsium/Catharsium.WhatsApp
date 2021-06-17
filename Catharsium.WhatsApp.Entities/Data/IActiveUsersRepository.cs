using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Data
{
    public interface IActiveUsersRepository
    {
        List<User> GetFor(string conversationName);
    }
}