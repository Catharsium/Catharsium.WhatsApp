using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Data;

public interface IExportUsersRepository
{
    List<User> GetForConversation(string conversationName);
}