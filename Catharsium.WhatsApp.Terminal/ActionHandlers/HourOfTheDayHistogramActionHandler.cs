using Catharsium.Math.Graphs.Interfaces;
using Catharsium.Math.Graphs.Models;
using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class HourOfTheDayHistogramActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IPeriodChooser periodChooser;
    private readonly IUserChooser userChooser;
    private readonly IGraph graph;
    private readonly IConsole console;

    public string DisplayName => "Hour of the Day Histogram";


    public HourOfTheDayHistogramActionHandler(
        IConversationChooser conversationChooser,
        IPeriodChooser periodChooser,
        IUserChooser userChooser,
        IGraph graph,
        IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.userChooser = userChooser;
        this.periodChooser = periodChooser;
        this.graph = graph;
        this.console = console;
    }


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var conversation = await this.conversationChooser.Run();
        var user = await this.userChooser.AskForUser(conversation.Name);

        var messages = conversation.Messages.Include(new PeriodFilter(period));
        if (user != null) {
            messages = messages.Include(new UserFilter(user));
        }

        var groups = messages.GroupBy(m => m.Timestamp.Hour).Select(g => new { g.First().Timestamp.Hour, Messages = g.Count() }).OrderBy(g => g.Hour);
        var graphData = new Dictionary<string, decimal>();
        foreach (var group in groups) {
            graphData.Add(group.Hour.ToString(), group.Messages);
        }
        for (var i = 0; i < 24; i++) {
            if (!graphData.ContainsKey(i.ToString())) {
                graphData[i.ToString()] = 0;
            }
        }

        var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
        var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

        this.console.WriteLine($"Last update:\t{messages.Max(m => m.Timestamp):dd-MM-yyyy (HH:mm)}");
        if (user != null) {
            this.console.WriteLine($"User:\t{user}");
        }
        this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
        this.console.WriteLine($"Message:\t{messages.Count()}");

        this.graph.Generate(new GraphData { Data = graphData.AsEnumerable().OrderBy(d => int.Parse(d.Key)), Size = 100 });
    }
}