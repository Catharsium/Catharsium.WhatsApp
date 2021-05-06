using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Entities.Terminal.Steps
{
    public interface IConversationChooser
    {
        Task<IEnumerable<Message>> AskAndLoad();
    }
}