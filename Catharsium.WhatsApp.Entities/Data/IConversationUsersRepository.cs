using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Data;

public interface IConversationUsersRepository
{
    Task<List<User>> Get(string conversationName);
    Task<List<User>> Add(IEnumerable<User> users, string conversationName);
    Task<List<User>> Remove(IEnumerable<User> users, string conversationName);
    Task Save();
}