using Catharsium.WhatsApp.Entities.Models;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Entities.Terminal.Steps
{
    public interface IPeriodChooser
    {
        Task<Period> AskForPeriod();
    }
}