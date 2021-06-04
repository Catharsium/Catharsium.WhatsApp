using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Data.Repository
{
    public class WhatsAppRepository : IWhatsAppRepository
    {
        private readonly IFileFactory fileFactory;
        private readonly IUsersRepository usersRepository;
        private readonly IConsole console;
        private readonly WhatsAppDataSettings settings;


        public WhatsAppRepository(IFileFactory fileFactory, IUsersRepository usersRepository, IConsole console, WhatsAppDataSettings settings)
        {
            this.fileFactory = fileFactory;
            this.usersRepository = usersRepository;
            this.console = console;
            this.settings = settings;
        }


        public IFile[] GetFiles()
        {
            return this.fileFactory.CreateDirectory(this.settings.DataFolder).GetFiles("*.txt");
        }


        public async Task<IEnumerable<Message>> GetMessages(IFile file)
        {
            var users = await this.usersRepository.ReadFrom(file.ExtensionlessName);
            var lines = new List<string>();
            using (var reader = file.OpenText()) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    lines.Add(line);
                }
            }

            var result = new List<Message>();
            var regex = new Regex(@"(\d+)/(\d+)/(\d+), (\d+):(\d+) - (.[^:]+): (.+)");

            Message lastMessage = null;
            foreach (var line in lines) {
                var match = regex.Match(line);
                if (match.Success) {
                    var month = int.Parse(match.Groups[1].Value);
                    var day = int.Parse(match.Groups[2].Value);
                    var year = int.Parse(match.Groups[3].Value) + 2000;
                    var hour = int.Parse(match.Groups[4].Value);
                    var minute = int.Parse(match.Groups[5].Value);
                    var sender = match.Groups[6].Value;
                    var text = match.Groups[7].Value;
                    lastMessage = new Message {
                        Timestamp = new DateTime(year, month, day, hour, minute, 0),
                        Sender = users.FirstOrDefault(u => u.Aliases.Any(a => a == sender) || u.PhoneNumber == sender),
                        Text = text
                    };
                    if (lastMessage.Sender == null) {
                        lastMessage.Sender = new User {
                            PhoneNumber = sender,
                            IsActive = false
                        };
                    }
                    result.Add(lastMessage);
                }
                else {
                    if (lastMessage != null) {
                        lastMessage.Text += "\n" + line;
                    }
                }
            }

            return result;
        }
    }
}