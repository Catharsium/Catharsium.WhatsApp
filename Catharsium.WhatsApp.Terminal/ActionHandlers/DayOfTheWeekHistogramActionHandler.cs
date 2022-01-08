using Catharsium.Math.Graph.Interfaces;
using Catharsium.Math.Graph.Models;
using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Terminal.Models;
using Catharsium.WhatsApp.Terminal.Models.Comparers;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class DayOfTheWeekHistogramActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IPeriodChooser periodChooser;
    private readonly IEqualityComparer<User> userComparer;
    private readonly IGraph graph;
    private readonly IConsole console;

    public string FriendlyName => "Day of the Week Histogram";


    public DayOfTheWeekHistogramActionHandler(
        IConversationChooser conversationChooser,
        IPeriodChooser periodChooser,
        IEqualityComparer<User> userComparer,
        IGraph graph,
        IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.periodChooser = periodChooser;
        this.userComparer = userComparer;
        this.graph = graph;
        this.console = console;
    }


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var messages = await this.conversationChooser.AskAndLoad();
        messages = messages.Include(new PeriodFilter(period));

        var users = messages.Select(m => m.Sender).Distinct(new UserEqualityComparer()).Where(u => u != null).OrderBy(u => messages.Where(m => m.Sender == u).Count()).ToList();
        var user = this.console.AskForItem(users);
        if (user != null) {
            messages = messages.Where(m => m.Sender != null).Include(new UserFilter(user));
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