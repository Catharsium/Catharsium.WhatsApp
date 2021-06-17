using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Entities.Data
{
    public interface IConversationsRepository
    {
        Task<List<Conversation>> GetConversations();
    }
}