using Catharsium.Util.IO.Files.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Data.Interfaces;

public interface IMessageParser
{
    Task<List<Message>> GetMessages(IFile file);
}