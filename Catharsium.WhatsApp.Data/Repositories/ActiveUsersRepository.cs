using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Terminal.Data;
using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Data.Repositories;

public class ActiveUsersRepository : IActiveUsersRepository
{
    private readonly WhatsAppDataSettings settings;


    public ActiveUsersRepository(WhatsAppDataSettings settings)
    {
        this.settings = settings;
    }


    public List<User> GetFor(string conversationName)
    {
        var result = new List<User>();
        if (this.settings.ActiveUsers.ContainsKey(conversationName)) {
            var list = this.settings.ActiveUsers[conversationName];
            result = list.Split(", ").Select(u => new User {
                Aliases = u.StartsWith('+') ? new List<string>() : new List<string> { u },
                PhoneNumber = u.StartsWith('+') ? u : ""
            }).ToList();
        }

        return result;
    }
}