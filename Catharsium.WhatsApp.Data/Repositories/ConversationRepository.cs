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
    public class ConversationRepository : IConversationRepository
    {
        private readonly IFileFactory fileFactory;
        private readonly IEqualityComparer<Message> messageEqualityComparer;
        private readonly IConsole console;
        private readonly WhatsAppDataSettings settings;


        public ConversationRepository(IFileFactory fileFactory, IEqualityComparer<Message> messageEqualityComparer, IConsole console, WhatsAppDataSettings settings)
        {
            this.fileFactory = fileFactory;
            this.messageEqualityComparer = messageEqualityComparer;
            this.console = console;
            this.settings = settings;
        }


        private IFile[] GetFiles()
        {
            return this.fileFactory.CreateDirectory(this.settings.DataFolder).GetFiles("*.txt");
        }


        public async Task<List<Conversation>> GetConversations()
        {
            var result = new List<Conversation>();
            var files = this.GetFiles();

            foreach (var file in files) {
                var regex = new Regex("^(.+) (\\d)$");
                var match = regex.Match(file.ExtensionlessName);
                var name = file.ExtensionlessName;
                if (match.Success && match.Groups.Count == 3) {
                    name = match.Groups[1].Value;
                }

                var existingConversation = result.FirstOrDefault(c => c.Name == name);
                if (existingConversation == null) {
                    existingConversation = new Conversation {
                        Name = name,
                        Files = new List<IFile> { file },
                        Messages = new List<Message>()
                    };
                    result.Add(existingConversation);
                }
                else {
                    existingConversation.Files.Add(file);
                }
            }

            return result;
        }



        public async Task<IEnumerable<Message>> GetMessages(Conversation conversation, IEnumerable<User> users)
        {
            var result = new List<Message>();
            foreach (var file in conversation.Files) {
                result.AddRange(await this.GetMessages(file, users));
            }

            return result.Distinct(this.messageEqualityComparer).ToList();
        }


        public async Task<IEnumerable<Message>> GetMessages(IFile file, IEnumerable<User> users)
        {
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