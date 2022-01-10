using Catharsium.Math.Graph.Interfaces;
using Catharsium.Math.Graph.Models;
using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using Catharsium.WordCloud.Interfaces;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class WordsActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IPeriodChooser periodChooser;
    private readonly IUserChooser userChooser;
    private readonly IWordCounter wordCounter;
    private readonly IGraph graph;
    private readonly IConsole console;

    public WordsActionHandler(
        IConversationChooser conversationChooser,
        IPeriodChooser periodChooser,
        IUserChooser userChooser,
        IWordCounter wordCounter,
        IGraph graph,
        IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.periodChooser = periodChooser;
        this.userChooser = userChooser;
        this.wordCounter = wordCounter;
        this.graph = graph;
        this.console = console;
    }


    public string FriendlyName => "Words";


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var conversation = await this.conversationChooser.AskForConversation();
        var user = await this.userChooser.AskForUser(conversation.Name);
        var specificWord = this.console.AskForText("Search for a specific word:");

        var messages = conversation.Messages.Include(new PeriodFilter(period));
        if (user != null) {
            messages = messages.Include(new UserFilter(user));
        }

        var texts = messages.Select(m => m.Text).Where(x => x != null).ToArray(); 
        this.wordCounter.Add(texts);
        var words = this.wordCounter.Words.OrderByDescending(w => w.Value);

        var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
        var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

        this.console.WriteLine($"Last update:\t{messages.Last().Timestamp:dd-MM-yyyy (HH:mm)}");
        if (user != null) {
            this.console.WriteLine($"User:\t{user}");
        }
        this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
        this.console.WriteLine($"Total words:\t{words.Count()}");

        if (!string.IsNullOrWhiteSpace(specificWord)) {
            var index = words.Select(w => w.Key).ToList().IndexOf(specificWord);
            if (index > 0 && index < words.Count()) {
                words = words.Skip(index - 5).Take(11).OrderByDescending(w => w.Value);
            }
        }

        var data = new GraphData {
            Data = words.Take(50).Select(w => new KeyValuePair<string, decimal>(w.Key, new decimal(w.Value)))
        };

        this.graph.Generate(data);
    }
}