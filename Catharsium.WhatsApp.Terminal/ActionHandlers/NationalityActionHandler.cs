using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Terminal.Models.Comparers;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class NationalityActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IPeriodChooser periodChooser;
    private readonly IConsole console;

    public string FriendlyName => "De Buren Strijd";


    public NationalityActionHandler(IConversationChooser conversationChooser, IPeriodChooser periodChooser, IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.periodChooser = periodChooser;
        this.console = console;
    }


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var messages = await this.conversationChooser.AskAndLoad();
        messages = messages.Include(new PeriodFilter(period));

        var groups = messages.GroupBy(m => m.Sender.PhoneNumber.Substring(0, 3));

        foreach (var group in groups) {
            var netCode = group.First().Sender.PhoneNumber.Substring(0, 3);
            var users = group.Select(m => m.Sender).Distinct(new UserEqualityComparer()).OrderBy(u => messages.Where(m => m.Sender == u).Count()).ToList();
            var messagesPerUser = group.Count() / (double)users.Count;
            this.console.WriteLine($"{netCode} ({users.Count} users) - {group.Count()} messages ({messagesPerUser:n2} per user)");
        }
    }
}