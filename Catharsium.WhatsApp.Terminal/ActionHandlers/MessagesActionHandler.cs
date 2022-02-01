using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class MessagesActionHandler : BaseActionHandler
{
    private readonly ISelectionActionStep<Conversation> conversationChooser;
    private readonly ISelectionActionStep<Period> periodChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;


    public MessagesActionHandler(
        ISelectionActionStep<Conversation> conversationChooser,
        ISelectionActionStep<Period> periodChooser,
        IConversationUsersRepository conversationUsersRepository,
        IConsole console)
        : base(console, "Messages")
    {
        this.conversationChooser = conversationChooser;
        this.periodChooser = periodChooser;
        this.conversationUsersRepository = conversationUsersRepository;
    }


    public override async Task Run()
    {
        var period = await this.periodChooser.Select();
        var conversation = await this.conversationChooser.Select();
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