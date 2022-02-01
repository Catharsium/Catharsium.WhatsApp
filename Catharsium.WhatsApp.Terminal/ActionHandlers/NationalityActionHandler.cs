using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class NationalityActionHandler : BaseActionHandler
{
    private readonly ISelectionActionStep<Conversation> conversationChooser;
    private readonly ISelectionActionStep<Period> periodChooser;


    public NationalityActionHandler(
        ISelectionActionStep<Conversation> conversationChooser,
        ISelectionActionStep<Period> periodChooser,
        IConsole console)
        : base(console, "De Buren Strijd")
    {
        this.conversationChooser = conversationChooser;
        this.periodChooser = periodChooser;
    }


    public override async Task Run()
    {
        var conversation = await this.conversationChooser.Select();
        var period = await this.periodChooser.Select();
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