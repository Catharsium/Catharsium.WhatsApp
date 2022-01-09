using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Steps;

public class PeriodChooser : IPeriodChooser
{
    private readonly IConsole console;


    public PeriodChooser(IConsole console)
    {
        this.console = console;
    }


    public async Task<Period> AskForPeriod()
    {
        return await Task.FromResult(
             new Period {
                 From = this.console.AskForDate("Enter the from date (yyyy MM dd)", DateTime.MinValue),
                 To = this.console.AskForDate("Enter the to date (yyyy MM dd)", DateTime.MaxValue)
             }
        );
    }
}