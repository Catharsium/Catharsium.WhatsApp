using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class MessagesActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IPeriodChooser periodChooser;
    private readonly IConsole console;

    public string DisplayName => "Messages";


    public MessagesActionHandler(
        IConversationChooser conversationChooser,
        IConversationUsersRepository conversationUsersRepository,
        IPeriodChooser periodChooser,
        IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.periodChooser = periodChooser;
        this.console = console;
    }


    public async Task Run()
    {
        var period = await this.periodChooser.AskForPeriod();
        var conversation = await this.conversationChooser.Run();
        var messages = conversation.Messages.Include(new PeriodFilter(period));

        var users = await this.conversationUsersRepository.Get(conversation.Name);
        var user = this.console.AskForItem(users);
        if (user != null) {
            messages = messages.Include(new UserFilter(user)).OrderByDescending(m => m.Timestamp);
        }

        foreach (var message in messages) {
            this.console.WriteLine($"{message}");
        }

        var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
        var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";
        if (user != null) {
            this.console.WriteLine($"User:\t{user}");
        }
        this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
        this.console.WriteLine($"Message:\t{messages.Count()}");
    }
}