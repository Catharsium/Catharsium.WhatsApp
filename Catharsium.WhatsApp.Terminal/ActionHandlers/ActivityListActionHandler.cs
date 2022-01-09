using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Terminal.Terminal.Steps;
using System.Globalization;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class ActivityListActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IPeriodChooser periodChooser;
    private readonly IConsole console;

    public string FriendlyName => "Activity list";


    public ActivityListActionHandler(IConversationChooser conversationChooser, IConversationUsersRepository conversationUsersRepository, IPeriodChooser periodChooser, IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.periodChooser = periodChooser;
        this.console = console;
    }


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var conversation = await this.conversationChooser.AskAndLoad();
        var messages = conversation.Messages.Include(new PeriodFilter(period));
        var users = await this.conversationUsersRepository.Get(conversation.Name);

        var statistics = new List<UserStatistics>();
        foreach (var user in users) {
            var userMessages = messages.Include(new UserFilter(user)).OrderBy(m => m.Timestamp);
            if (userMessages.Any()) {
                var userStatistics = new UserStatistics(user, userMessages);
                statistics.Add(userStatistics);
            }
        }
        statistics = statistics.OrderByDescending(s => s.MessagesPerDay).ToList();
        var referenceDate = messages.Last().Timestamp;
        var dutchCulture = new CultureInfo("nl-NL");

        var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
        var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

        this.console.WriteLine($"Users:\t\t{users.Count}");
        this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
        this.console.WriteLine($"\t<Name>\t\t<Messages/Day>\t<Characters>\t<Messages>\t<MessageLength>\t<Last Message>");
        this.console.WriteLine();

        for (var i = 0; i < statistics.Count; i++) {
            this.WriteUser(statistics[i], i + 1, referenceDate, dutchCulture);
        }
    }


    private void WriteUser(UserStatistics statistics, int rank, DateTime referenceDate, CultureInfo dutchCulture)
    {
        string text;
        this.console.Write(text = $"{rank}.");
        this.FillSpaces(text.Length, 10);
        this.console.Write(text = $"{statistics.User}");
        this.FillSpaces(text.Length, 16);
        this.console.ForegroundColor = ConsoleColor.DarkGreen;
        this.console.Write(text = $"{statistics.MessagesPerDay:0}");
        this.FillSpaces(text.Length, 16);
        this.console.ForegroundColor = ConsoleColor.DarkYellow;
        this.console.Write(text = $"{statistics.TotalCharacters:n0}");
        this.FillSpaces(text.Length, 16);
        this.console.ForegroundColor = ConsoleColor.DarkGreen;
        this.console.Write(text = $"{statistics.TotalMessages}");
        this.FillSpaces(text.Length, 17);
        this.console.ForegroundColor = ConsoleColor.DarkYellow;
        this.console.Write(text = $"{statistics.AverageMessageLength:n0}");
        this.FillSpaces(text.Length);
        this.console.ResetColor();

        if (statistics.LastMessage.Timestamp.AddDays(7) < referenceDate) {
            this.console.ForegroundColor = ConsoleColor.Yellow;
        }
        if (statistics.LastMessage.Timestamp.AddDays(14) < referenceDate) {
            this.console.ForegroundColor = ConsoleColor.Red;
        }
        this.console.Write($"{statistics.LastMessage.Timestamp.ToString("d MMMMM yyyy", dutchCulture)}");
        this.console.ResetColor();
        this.console.WriteLine();
    }


    private void FillSpaces(int length, int total = 15)
    {
        for (var i = length; i < total; i++) {
            this.console.Write(" ");
        }
    }
}