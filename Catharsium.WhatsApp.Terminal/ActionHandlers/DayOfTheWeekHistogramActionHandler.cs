using Catharsium.Math.Graph.Interfaces;
using Catharsium.Math.Graph.Models;
using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class DayOfTheWeekHistogramActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IPeriodChooser periodChooser;
    private readonly IGraph graph;
    private readonly IConsole console;

    public string FriendlyName => "Day of the Week Histogram";


    public DayOfTheWeekHistogramActionHandler(
        IConversationChooser conversationChooser,
        IConversationUsersRepository conversationUsersRepository,
        IPeriodChooser periodChooser,
        IGraph graph,
        IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.periodChooser = periodChooser;
        this.graph = graph;
        this.console = console;
    }


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var conversation = await this.conversationChooser.AskAndLoad();
        var messages = conversation.Messages.Include(new PeriodFilter(period));

        var users = await this.conversationUsersRepository.Get(conversation.Name);
        var user = this.console.AskForItem(users);
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