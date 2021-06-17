using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Data.Repository
{
    public class ConversationUsersRepository : IConversationUsersRepository
    {
        private readonly IFileFactory fileFactory;
        private readonly IJsonFileReader jsonFileReader;
        private readonly IJsonFileWriter jsonFileWriter;
        private readonly IConsole console;
        private readonly WhatsAppDataSettings settings;


        public ConversationUsersRepository(
            IFileFactory fileFactory,
            IJsonFileReader jsonFileReader,
            IJsonFileWriter jsonFileWriter,
            IConsole console,
            WhatsAppDataSettings settings)
        {
            this.fileFactory = fileFactory;
            this.jsonFileReader = jsonFileReader;
            this.jsonFileWriter = jsonFileWriter;
            this.console = console;
            this.settings = settings;
        }


        private IFile GetFile(string fileName)
        {
            return this.fileFactory.CreateFile($"{this.settings.DataFolder}/Users/{fileName}.json");
        }


        public async Task<IEnumerable<User>> ReadFrom(string fileName)
        {
            var file = this.GetFile(fileName);
            return this.jsonFileReader.ReadFrom<IEnumerable<User>>(file.FullName);
        }


        public async Task UpdateTo(IEnumerable<User> users, string fileName)
        {
            var file = this.GetFile(fileName);
            var currentUsers = new List<User>();
            if (file != null && file.Exists) {
                currentUsers = (await this.ReadFrom(fileName)).ToList();
                file.Delete();
            }

            var newUsers = new List<User>();
            foreach (var activeUser in users) {
                var currentUser = currentUsers.FirstOrDefault(u =>
                    u.Aliases.Any(currentAlias => activeUser.Aliases.Any(activeAlias => currentAlias == activeAlias)) ||
                    (!string.IsNullOrEmpty(u.PhoneNumber) && u.PhoneNumber == activeUser.PhoneNumber));
                User newUser;
                if (currentUser == null) {
                    newUser = new User {
                        PhoneNumber = activeUser.PhoneNumber,
                        DisplayName = activeUser.DisplayName,
                        Aliases = activeUser.Aliases ?? new List<string>(),
                        IsActive = true
                    };
                    this.console.WriteLine($"Found new user '{newUser}'");
                }
                else {
                    newUser = new User {
                        PhoneNumber = currentUser.PhoneNumber,
                        DisplayName = this.GetNonPhoneNumber(currentUser.DisplayName),
                        Aliases = new List<string>(),
                        IsActive = true
                    };
                    if (currentUser.Aliases != null) {
                        newUser.Aliases.AddRange(currentUser.Aliases);
                    }
                    currentUsers.Remove(currentUser);
                }
                newUser.Aliases = newUser.Aliases.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
                newUsers.Add(newUser);
            }

            foreach (var currentUser in currentUsers) {
                this.console.WriteLine($"Deleting user '{currentUser}' ");
            }

            this.jsonFileWriter.Write(newUsers, file.FullName);
        }


        private string GetNonPhoneNumber(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && !name.StartsWith('+')
                ? name
                : "";
        }
    }
}