using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Data
{
    public interface IWhatsAppRepository
    {
        IFile[] GetFiles();

        IEnumerable<Message> GetMessages(IFile file);
    }
}