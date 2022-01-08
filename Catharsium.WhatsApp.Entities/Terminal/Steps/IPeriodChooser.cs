using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Terminal.Terminal.Steps;

public interface IPeriodChooser
{
    Task<Period> AskForPeriod();
}