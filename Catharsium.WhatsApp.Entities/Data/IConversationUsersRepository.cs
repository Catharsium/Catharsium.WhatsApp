using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Data.Repository
{
    public interface IConversationUsersRepository
    {
        Task<List<User>> Get(string conversationName);

        Task<List<User>> Add(IEnumerable<User> users, string conversationName);

        Task<List<User>> Remove(IEnumerable<User> users, string conversationName);
    }
}