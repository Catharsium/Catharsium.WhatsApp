using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models.Comparers;
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
        var conversation = await this.conversationChooser.AskAndLoad();
        var period = await this.periodChooser.AskForPeriod();
        var messages = conversation.Messages.Include(new PeriodFilter(period));

        //var groups = messages.GroupBy(m => m.Sender.PhoneNumber[..3]);

        //foreach (var group in groups) {
        //    var netCode = group.First().Sender.PhoneNumber[..3];
        //    var users = group.Select(m => m.Sender).Distinct(new UserEqualityComparer()).OrderBy(u => messages.Where(m => m.Sender == u).Count()).ToList();
        //    var messagesPerUser = group.Count() / (double)users.Count;
        //    this.console.WriteLine($"{netCode} ({users.Count} users) - {group.Count()} messages ({messagesPerUser:n2} per user)");
        //}
    }
}