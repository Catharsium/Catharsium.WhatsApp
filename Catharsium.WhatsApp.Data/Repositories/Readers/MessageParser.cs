using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Data.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
using System.Text.RegularExpressions;
namespace Catharsium.WhatsApp.Data.Repositories.Readers;

public class MessageParser : IMessageParser
{
    private readonly IEqualityComparer<Message> messageEqualityComparer;


    public MessageParser(IEqualityComparer<Message> messageEqualityComparer)
    {
        this.messageEqualityComparer = messageEqualityComparer;
    }


    public Task<List<Message>> GetMessages(IFile file)
    {
        return Task.Run(() => {
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
                        Sender = sender,
                        Text = text
                    };
                    result.Add(lastMessage);
                }
                else {
                    if (lastMessage != null) {
                        lastMessage.Text += "\n" + line;
                    }
                }
            }

            return result.Distinct(this.messageEqualityComparer).ToList();
        });
    }
}