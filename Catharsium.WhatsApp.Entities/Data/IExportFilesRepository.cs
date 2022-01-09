using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Data;

public interface IExportFilesRepository
{
    Task<List<Conversation>> GetConversations();
}