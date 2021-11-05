using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Data.Repository
{
    public class ConversationsRepository : IConversationsRepository
    {
        private readonly IFileFactory fileFactory;
        private readonly WhatsAppDataSettings settings;


        public ConversationsRepository(IFileFactory fileFactory, WhatsAppDataSettings settings)
        {
            this.fileFactory = fileFactory;
            this.settings = settings;
        }


        private Task<IFile[]> GetFiles()
        {
            return Task.FromResult(this.fileFactory.CreateDirectory(this.settings.DataFolder).GetFiles("*.txt"));
        }


        public async Task<List<Conversation>> GetConversations()
        {
            var result = new List<Conversation>();
            var files = await this.GetFiles();

            foreach (var file in files) {
                var name = this.GetConversationName(file);
                var existingConversation = result.FirstOrDefault(c => c.Name == name);
                if (existingConversation == null) {
                    result.Add(new Conversation {
                        Name = name,
                        EportFiles = new List<IFile> { file },
                        Messages = new List<Message>()
                    });
                }
                else {
                    existingConversation.EportFiles.Add(file);
                }
            }

            return result;
        }


        private string GetConversationName(IFile file)
        {
            var regex = new Regex("^(.+) (\\d+)$");
            var match = regex.Match(file.ExtensionlessName);
            var name = file.ExtensionlessName;
            if (match.Success && match.Groups.Count == 3) {
                name = match.Groups[1].Value;
            }

            return name;
        }
    }
}