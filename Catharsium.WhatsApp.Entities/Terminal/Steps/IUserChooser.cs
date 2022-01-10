using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Terminal.Steps;

public interface IUserChooser
{
    Task<User> AskForUser(string conversationName);
}