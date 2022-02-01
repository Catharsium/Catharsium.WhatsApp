using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class ReportActionHandler : BaseActionHandler
{
    public ReportActionHandler(IConsole console)
        : base(console, "Rapport")
    { }


    public override async Task Run()
    {
        //var activityList = new ActivityListActionHandler();
    }
}