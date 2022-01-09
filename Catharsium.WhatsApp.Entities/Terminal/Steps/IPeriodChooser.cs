using Catharsium.WhatsApp.Entities.Models;

namespace Catharsium.WhatsApp.Terminal.Terminal.Steps;

public interface IPeriodChooser
{
    Task<Period> AskForPeriod();
}