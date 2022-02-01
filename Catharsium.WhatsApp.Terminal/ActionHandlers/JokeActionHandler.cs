using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.ActionHandlers.Interfaces;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Terminal._Configuration;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class JokeActionHandler : BaseActionHandler
{
    private readonly ISelectionActionStep<Conversation> conversationChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly WhatsAppTerminalSettings settings;


    public JokeActionHandler(
        ISelectionActionStep<Conversation> conversationChooser,
        IConversationUsersRepository conversationUsersRepository,
        IConsole console,
        WhatsAppTerminalSettings settings)
        : base(console, "Joke")
    {
        this.conversationChooser = conversationChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.settings = settings;
    }


    public override async Task Run()
    {
        var userAlias = this.settings.JokeAction["User alias"];
        var conversation = await this.conversationChooser.Select();
        var users = await this.conversationUsersRepository.Get(conversation.Name);
        var maxNameLength = users.Max(u => u.ToString().Length);

        this.console.WriteLine(this.settings.JokeAction["Title"]);
        foreach (var user in users) {
            if (user.DisplayName == userAlias) {
                continue;
            }
            this.console.Write($"[1]  ");
            this.console.Write(user.ToString());
            this.console.FillBlock(user.ToString().Length, maxNameLength + 5);
            this.console.WriteLine("100%");
        }

        this.console.Write($"[{users.Count()}] ");
        this.console.Write(userAlias);
        this.console.FillBlock(userAlias.Length, maxNameLength + 5);
        this.console.WriteLine("  0%");
    }
}