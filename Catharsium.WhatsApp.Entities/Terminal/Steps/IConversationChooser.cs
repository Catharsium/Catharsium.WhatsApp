using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Terminal.Terminal.Steps;

public interface IConversationChooser
{
    Task<IEnumerable<Message>> AskAndLoad();
}