using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers.Steps;

public class PeriodChooser : ISelectionActionStep<Period>
{
    private readonly IConsole console;


    public PeriodChooser(IConsole console)
    {
        this.console = console;
    }


    public async Task<Period> Select()
    {
        return await Task.FromResult(
             new Period {
                 From = this.console.AskForDate("Enter the from date (yyyy MM dd)", DateTime.MinValue),
                 To = this.console.AskForDate("Enter the to date (yyyy MM dd)", DateTime.MaxValue)
             }
        );
    }
}