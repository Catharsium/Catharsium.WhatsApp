using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using Catharsium.WhatsApp.Terminal._Configuration;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class JokeActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IConsole console;
    private readonly WhatsAppTerminalSettings settings;

    public string DisplayName => "Joke";


    public JokeActionHandler(
        IConversationChooser conversationChooser,
        IConversationUsersRepository conversationUsersRepository,
        IConsole console,
        WhatsAppTerminalSettings settings)
    {
        this.conversationChooser = conversationChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.console = console;
        this.settings = settings;
    }


    public async Task Run()
    {
        var userAlias = this.settings.JokeAction["User alias"];
        var conversation = await this.conversationChooser.Run();
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

        this.console.Write($"[{users.Count}] ");
        this.console.Write(userAlias);
        this.console.FillBlock(userAlias.Length, maxNameLength + 5);
        this.console.WriteLine("  0%");
    }
}