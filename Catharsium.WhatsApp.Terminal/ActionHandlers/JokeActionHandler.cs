using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
namespace Catharsium.WhatsApp.Terminal.ActionHandlers;

public class JokeActionHandler : IActionHandler
{
    private readonly IConversationChooser conversationChooser;
    private readonly IConversationUsersRepository conversationUsersRepository;
    private readonly IConsole console;

    public string FriendlyName => "Joke";


    public JokeActionHandler(IConversationChooser conversationChooser, IConversationUsersRepository conversationUsersRepository, IConsole console)
    {
        this.conversationChooser = conversationChooser;
        this.conversationUsersRepository = conversationUsersRepository;
        this.console = console;
    }


    public async Task Run()
    {
        var conversation = await this.conversationChooser.AskForConversation();
        var users = await this.conversationUsersRepository.Get(conversation.Name);
        var maxName = users.Max(u => u.ToString().Length);
        this.console.WriteLine("Sexyness Per User");
        foreach (var user in users) {
            if (user.DisplayName == "Bart van L.") {
                continue;
            }
            this.console.Write($"[1]  ");
            this.console.Write(user.ToString());
            this.console.FillBlock(user.ToString().Length, maxName + 5);
            this.console.WriteLine("100%");
        }

        this.console.Write($"[{users.Count}] ");
        this.console.Write("Bart van L.");
        this.console.FillBlock("Bart van L.".Length, maxName + 5);
        this.console.WriteLine("  0%");
    }
}