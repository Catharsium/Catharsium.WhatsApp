using Catharsium.WhatsApp.Terminal.Models;

namespace Catharsium.WhatsApp.Terminal.Data;

public interface IActiveUsersRepository
{
    List<User> GetFor(string conversationName);
}