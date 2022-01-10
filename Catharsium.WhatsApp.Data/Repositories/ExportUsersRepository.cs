using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Data.Repositories;

public class ExportUsersRepository : IExportUsersRepository
{
    private readonly WhatsAppDataSettings settings;


    public ExportUsersRepository(WhatsAppDataSettings settings)
    {
        this.settings = settings;
    }


    public List<User> GetForConversation(string conversationName)
    {
        var result = new List<User>();
        if (this.settings.ActiveUsers.ContainsKey(conversationName)) {
            var list = this.settings.ActiveUsers[conversationName];
            result = list.Split(", ")
                         .Select(u => new User(u))
                         .ToList();
        }

        return result;
    }
}