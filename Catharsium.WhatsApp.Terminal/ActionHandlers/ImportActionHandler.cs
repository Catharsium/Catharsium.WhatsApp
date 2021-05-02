using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Ui.Terminal.ActionHandlers
{
    public class ImportActionHandler : IActionHandler
    {
        private readonly IWhatsAppExportFile dataFile;
        private readonly IConsole console;

        public string FriendlyName => "Import";


        public ImportActionHandler(IWhatsAppExportFile dataFile, IConsole console)
        {
            this.dataFile = dataFile;
            this.console = console;
        }


        public async Task Run()
        {
            var messages = this.dataFile.GetMessages().OrderBy(m => m.Date);
            var users = messages.Select(m => m.Sender).Distinct(new UserEqualityComparer()).OrderBy(u => messages.Where(m => m.Sender == u).Count()).ToList();
            var statistics = users.Select(u => new UserStatistics(u, messages.Where(m => m.Sender == u).OrderBy(m => m.Date))).OrderByDescending(s => s.MessagesPerDay);
            var activeUsers = statistics.Where(u => u.User.IsActive);
            var referenceDate = messages.Last().Date;
            var dutchCulture = new CultureInfo("nl-NL");

            this.console.WriteLine($"Last update: {messages.Last().Date:dd-MM-yyyy (HH:mm)}");
            this.console.WriteLine($"Users: {activeUsers.Count()}");
            this.console.WriteLine($"\t<Name>\t\t<Messages/Day>\t<Characters>\t<Messages>\t<MessageLength>\t<Last Message>");
            var counter = 1;
            foreach (var user in activeUsers) {
                this.WriteUser(user, counter++, referenceDate, dutchCulture);
            }
        }


        private void WriteUser(UserStatistics statistics, int rank, DateTime referenceDate, CultureInfo dutchCulture)
        {
            string text;
            this.console.Write(text = $"{rank}.");
            this.FillSpaces(text.Length, 8);
            this.console.Write(text = $"{statistics.User}");
            this.FillSpaces(text.Length, 16);
            this.console.ForegroundColor = ConsoleColor.DarkGreen;
            this.console.Write(text = $"{statistics.MessagesPerDay:0}");
            this.FillSpaces(text.Length, 16);
            this.console.ForegroundColor = ConsoleColor.DarkYellow;
            this.console.Write(text = $"{statistics.TotalCharacters:n0}");
            this.FillSpaces(text.Length, 16);
            this.console.ForegroundColor = ConsoleColor.DarkGreen;
            this.console.Write(text = $"{statistics.TotalMessages}");
            this.FillSpaces(text.Length, 17);
            this.console.ForegroundColor = ConsoleColor.DarkYellow;
            this.console.Write(text = $"{statistics.AverageMessageLength:n0}");
            this.FillSpaces(text.Length);
            this.console.ResetColor();

            if (statistics.LastMessage.Date.AddDays(7) < referenceDate) {
                this.console.ForegroundColor = ConsoleColor.Red;
            }
            if (statistics.LastMessage.Date.AddDays(14) < referenceDate) {
                this.console.ForegroundColor = ConsoleColor.Red;
            }
            this.console.Write($"{statistics.LastMessage.Date.ToString("d MMMMM yyyy", dutchCulture)}");
            this.console.ResetColor();
            this.console.WriteLine();
        }


        private void FillSpaces(int length, int total = 15)
        {
            for (var i = length ; i < total ; i++) {
                this.console.Write(" ");
            }
        }
    }
}