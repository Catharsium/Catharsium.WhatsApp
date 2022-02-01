using Catharsium.Math.Graphs.Interfaces;
using Catharsium.Math.Graphs.Models;
using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class DayOfTheWeekHistogramActionHandler : BaseActionHandler
{
    private readonly ISelectionActionStep<Conversation> conversationChooser;
    private readonly ISelectionActionStep<Period> periodChooser;
    private readonly IUserChooser userChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IGraph graph;


    public DayOfTheWeekHistogramActionHandler(
        ISelectionActionStep<Conversation> conversationChooser,
        ISelectionActionStep<Period> periodChooser,
        IUserChooser userChooser,
        IConversationUsersRepository conversationUsersRepository,
        IGraph graph,
        IConsole console)
        : base(console, "Day of the Week Histogram")
    {
        this.conversationChooser = conversationChooser;
        this.periodChooser = periodChooser;
        this.userChooser = userChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.graph = graph;
    }


    public override async Task Run()
    {
        var period = await this.periodChooser.Select();
        var conversation = await this.conversationChooser.Select();
        var user = await this.userChooser.AskForUser(conversation.Name);

        var messages = conversation.Messages.Include(new PeriodFilter(period));
        if (user != null) {
            messages = messages.Include(new UserFilter(user));
        }

        var groups = messages.GroupBy(m => m.Timestamp.DayOfWeek).Select(g => new { g.First().Timestamp.DayOfWeek, Messages = g.Count() }).OrderBy(g => g.DayOfWeek);
        var graphData = new Dictionary<string, decimal>();
        foreach (var group in groups) {
            graphData.Add(group.DayOfWeek.ToString(), group.Messages);
        }

        var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
        var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

        this.console.WriteLine($"Last update:\t{messages.Last().Timestamp:dd-MM-yyyy (HH:mm)}");
        if (user != null) {
            this.console.WriteLine($"User:\t{user}");
        }
        this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
        this.console.WriteLine($"Message:\t{messages.Count()}");

        this.graph.Generate(new GraphData { Data = graphData.AsEnumerable(), Size = 100 });
    }
}