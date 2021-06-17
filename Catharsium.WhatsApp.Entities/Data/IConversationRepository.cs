using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Entities.Data
{
    public interface IConversationRepository
    {
        Task<List<Conversation>> GetConversations();

        Task<IEnumerable<Message>> GetMessages(Conversation conversation, IEnumerable<User> users);
    }
}