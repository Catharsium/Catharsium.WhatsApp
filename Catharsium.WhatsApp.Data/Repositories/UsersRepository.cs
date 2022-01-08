using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Terminal.Data;
using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Data.Repositories;

public class UsersRepository : IConversationUsersRepository
{
    private readonly IFileFactory fileFactory;
    private readonly IJsonFileReader jsonFileReader;
    private readonly IJsonFileWriter jsonFileWriter;
    private readonly IEqualityComparer<User> userEqualityComparer;
    private readonly IConsole console;
    private readonly WhatsAppDataSettings settings;


    private List<User> _users;
    private List<User> Users
    {
        get {
            if (this._users == null) {
                this._users = this.ReadAll();
            }
            return this._users;
        }
    }


    public UsersRepository(
        IFileFactory fileFactory,
        IJsonFileReader jsonFileReader,
        IJsonFileWriter jsonFileWriter,
        IEqualityComparer<User> userEqualityComparer,
        IConsole console,
        WhatsAppDataSettings settings)
    {
        this.fileFactory = fileFactory;
        this.jsonFileReader = jsonFileReader;
        this.jsonFileWriter = jsonFileWriter;
        this.userEqualityComparer = userEqualityComparer;
        this.console = console;
        this.settings = settings;
    }


    private List<User> ReadAll()
    {
        var result = new List<User>();
        var file = this.GetFile();
        if (file.Exists) {
            result = this.jsonFileReader.ReadFrom<IEnumerable<User>>(file.FullName).ToList();
        }
        return result;
    }


    private IFile GetFile()
    {
        return this.fileFactory.CreateFile($"{this.settings.DataFolder}/Data/AllUsers.json");
    }


    public async Task<List<User>> Get(string conversationName)
    {
        return await Task.FromResult(this.Users.Where(u => u.Conversations.Any(c => c == conversationName)).ToList());
    }


    public async Task<List<User>> Add(IEnumerable<User> users, string conversationName)
    {
        var newUsers = new List<User>();
        foreach (var activeUser in users) {
            this.console.WriteLine($"Handling user {activeUser} for conversation {conversationName}");
            var currentUser = this.GetCurrentUser(activeUser, conversationName);
            User newUser;
            if (currentUser == null) {
                newUser = new User {
                    PhoneNumber = activeUser.PhoneNumber,
                    DisplayName = activeUser.DisplayName,
                    Aliases = activeUser.Aliases ?? new List<string>(),
                    Conversations = new List<string> { conversationName }
                };
                this.console.WriteLine($"Adding new user '{newUser}'");
            }
            else {
                newUser = new User {
                    PhoneNumber = currentUser.PhoneNumber,
                    DisplayName = GetIfNonPhoneNumber(currentUser.DisplayName),
                    Aliases = new List<string>(),
                    Conversations = currentUser.Conversations
                };
                if (newUser.Conversations.All(c => c != conversationName)) {
                    newUser.Conversations.Add(conversationName);
                }
                newUser.Aliases.AddRange(currentUser.Aliases);
                this.Users.Remove(currentUser);
            }

            newUser.Aliases = newUser.Aliases.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            newUsers.Add(newUser);
            this.Users.Add(newUser);
        }

        return newUsers;
    }


    public async Task<List<User>> Remove(IEnumerable<User> users, string conversationName)
    {
        throw new NotImplementedException();
    }


    private Task WriteAll()
    {
        return Task.Run(() => {
            var file = this.GetFile();
            if (file.Exists) {
                file.Delete();
            }

            this.jsonFileWriter.Write(this.Users, file.FullName);
        });
    }


    private User GetCurrentUser(User activeUser, string conversationName)
    {
        this.console.WriteLine($"Getting current user for {activeUser}");
        var result = this.Users.FirstOrDefault(u => this.userEqualityComparer.Equals(u, activeUser));
        if (result != null) {
            return result;
        }

        var aliasUsers = new List<User>();
        foreach (var alias in activeUser.Aliases) {
            aliasUsers.AddRange(this.GetByAlias(alias));
        }

        result = aliasUsers.FirstOrDefault(u => u.Conversations.Any(c => c == conversationName));
        if (result != null) {
            return result;
        }

        if (aliasUsers.Count == 1) {
            this.console.WriteLine($"Using {aliasUsers.First()} for {activeUser}");
            return aliasUsers.First();
        }

        return aliasUsers.Count == 0
            ? null
            : this.console.AskForItem(aliasUsers);
    }


    private IEnumerable<User> GetByAlias(string alias)
    {
        return this.Users.Where(u => u.Aliases.Any(a => alias == a));
    }


    private static string GetIfNonPhoneNumber(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && !name.StartsWith('+')
            ? name
            : "";
    }
}