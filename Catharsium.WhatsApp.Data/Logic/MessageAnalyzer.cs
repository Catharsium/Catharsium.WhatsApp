using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using System;
using System.Collections.Generic;
using System.Unicode;

namespace Catharsium.WhatsApp.Data.Logic
{
    public class MessageAnalyzer : IMessageAnalyzer
    {
        private readonly IConsole console;


        public MessageAnalyzer(IConsole console)
        {
            this.console = console;
        }


        public List<string> GetWords(string text)
        {
            throw new NotImplementedException();
        }


        public List<string> GetCharacters(string text)
        {
            throw new NotImplementedException();
        }


        public List<char> GetEmoticons(string text)
        {
            foreach (var character in text.ToCharArray()) {
                var x = UnicodeInfo.GetCharInfo(character).EmojiProperties;
                if (x != 0) {
                    ;
                }
            }
            return null;
        }


        public List<string> GetUrls(string text)
        {
            throw new NotImplementedException();
        }
    }
}