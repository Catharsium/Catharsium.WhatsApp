using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Terminal.Data;

public interface IMessageParser
{
    Task<IEnumerable<Message>> GetMessages(Conversation conversation, IEnumerable<User> users);
    Task<IEnumerable<Message>> GetMessages(IFile file, IEnumerable<User> users);
}