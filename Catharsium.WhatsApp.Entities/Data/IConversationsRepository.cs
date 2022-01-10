using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Data;

public interface IConversationsRepository
{
    Task<List<string>> GetList();
    Task<Conversation> Get(string name);
    Task Save(Conversation conversation);
}