using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Terminal.Data;

public interface IConversationsRepository
{
    Task<List<Conversation>> GetConversations();
}