using Catharsium.Util.Filters;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Data.Filters;

public class UserFilter : IFilter<Message>
{
    private readonly User[] users;


    public UserFilter(params User[] users)
    {
        this.users = users;
    }


    public bool Includes(Message item)
    {
        return this.users.Any(u => u.PhoneNumber == item.Sender || u.Aliases.Any(a => a == item.Sender));
    }
}