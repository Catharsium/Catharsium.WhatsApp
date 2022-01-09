using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Data;

public interface IConversationRepository
{
    Task<List<string>> GetList();
    Task<Conversation> Get(string name);
    Task Save(Conversation conversation);
}