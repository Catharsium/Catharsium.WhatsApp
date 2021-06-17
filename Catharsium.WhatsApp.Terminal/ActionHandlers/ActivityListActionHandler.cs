using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Models.Comparers;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class ActivityListActionHandler : IActionHandler
    {
        private readonly IConversationChooser conversationChooser;
        private readonly IPeriodChooser periodChooser;
        private readonly IConsole console;

        public string FriendlyName => "Activity list";


        public ActivityListActionHandler(IConversationChooser conversationChooser, IPeriodChooser periodChooser, IConsole console)
        {
            this.conversationChooser = conversationChooser;
            this.periodChooser = periodChooser;
            this.console = console;
        }


        public async Task Run()
        {
            var period = await this.periodChooser.AskForPeriod();
            var messages = await this.conversationChooser.AskAndLoad();
            messages = messages.Include(new PeriodFilter(period));

            var x = messages.Select(m => m.Sender);
            var y = x.Where(a => string.IsNullOrWhiteSpace(a.PhoneNumber));
            var users = messages.Select(m => m.Sender).Where(u => u.IsActive).Distinct(new UserEqualityComparer()).OrderBy(u => messages.Where(m => m.Sender == u).Count()).ToList();
            var statistics = users.Select(u => new UserStatistics(u, messages.Where(m => m.Sender == u).OrderBy(m => m.Timestamp))).OrderByDescending(s => s.MessagesPerDay);
            var referenceDate = messages.Last().Timestamp;
            var dutchCulture = new CultureInfo("nl-NL");

            var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
            var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

            this.console.WriteLine($"Last update:\t{messages.Last().Timestamp:dd-MM-yyyy (HH:mm)}");
            this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
            this.console.WriteLine($"Message:\t{messages.Count()}");
            this.console.WriteLine($"Users:\t\t{users.Count}");
            this.console.WriteLine($"\t<Name>\t\t<Messages/Day>\t<Characters>\t<Messages>\t<MessageLength>\t<Last Message>");
            var counter = 1;
            foreach (var user in statistics) {
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

            if (statistics.LastMessage.Timestamp.AddDays(7) < referenceDate) {
                this.console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (statistics.LastMessage.Timestamp.AddDays(14) < referenceDate) {
                this.console.ForegroundColor = ConsoleColor.Red;
            }
            this.console.Write($"{statistics.LastMessage.Timestamp.ToString("d MMMMM yyyy", dutchCulture)}");
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