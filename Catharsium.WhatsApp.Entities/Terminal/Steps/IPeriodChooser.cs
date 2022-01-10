using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Entities.Terminal.Steps;

public interface IPeriodChooser
{
    Task<Period> AskForPeriod();
}