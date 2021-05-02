using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Catharsium.WhatsApp.Data
{
    public class WhatsAppExportFile : IWhatsAppExportFile
    {
        private readonly WhatsAppDataSettings settings;


        public WhatsAppExportFile(WhatsAppDataSettings settings)
        {
            this.settings = settings;
        }


        public IEnumerable<Message> GetMessages()
        {
            var result = new List<Message>();
            var regex = new Regex(@"(\d)+/(\d+)/(\d+), (\d+):(\d+) - (.[^:]+): (.+)");
            //var lines = File.ReadAllLines(@"E:\Cloud\OneDrive\BE 2.0.txt"); 
            var lines = File.ReadAllLines(@"E:\Cloud\OneDrive\WhatsApp Chat with Poly safe place.txt");
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
                        Date = new DateTime(year, month, day, hour, minute, 0),
                        Sender = this.settings.Users.FirstOrDefault(u => u.NickName == sender),
                        Text = text
                    };
                    if (lastMessage.Sender == null) {
                        ;
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