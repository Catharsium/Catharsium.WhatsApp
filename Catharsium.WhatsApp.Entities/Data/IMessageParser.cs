using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Entities.Data
{
    public interface IMessageParser
    {
        Task<IEnumerable<Message>> GetMessages(Conversation conversation, IEnumerable<User> users);
        Task<IEnumerable<Message>> GetMessages(IFile file, IEnumerable<User> users);
    }
}