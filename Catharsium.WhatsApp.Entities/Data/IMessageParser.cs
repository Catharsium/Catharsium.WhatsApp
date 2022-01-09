using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Data;

public interface IMessageParser
{
    Task<List<Message>> GetMessages(IFile file);
}