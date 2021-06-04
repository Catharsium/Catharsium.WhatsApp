using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Entities.Data
{
    public interface IWhatsAppRepository
    {
        IFile[] GetFiles();

        Task<IEnumerable<Message>> GetMessages(IFile file);
    }
}