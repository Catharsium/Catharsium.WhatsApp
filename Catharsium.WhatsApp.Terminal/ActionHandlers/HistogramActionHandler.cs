using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class HistogramActionHandler : IActionHandler
    {
        private readonly IConversationChooser conversationChooser;
        private readonly IPeriodChooser periodChooser;
        private readonly IConsole console;

        public string FriendlyName => "Histogram";


        public HistogramActionHandler(IConversationChooser conversationChooser, IPeriodChooser periodChooser, IConsole console)
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

            var groups = messages.GroupBy(m => m.Timestamp.Hour).Select(g => new { g.First().Timestamp.Hour, Messages = g.Count() }).OrderBy(g => g.Hour);
            var max = groups.Max(g => g.Messages);
            var maxLength = 100;

            var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
            var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

            this.console.WriteLine($"Last update:\t{messages.Last().Timestamp:dd-MM-yyyy (HH:mm)}");
            this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
            this.console.WriteLine($"Message:\t{messages.Count()}");

            foreach (var group in groups) {
                var percentage = group.Messages / (double)max;
                var length = Math.Round(percentage * maxLength);
                var text = $"{group.Hour}{group.Messages:n0}";
                this.console.Write($"{group.Hour}");
                this.console.Write($" (");
                this.console.ForegroundColor = ConsoleColor.DarkGreen;
                this.console.Write($"{group.Messages:n0}");
                this.console.ResetColor();
                this.console.Write($")");
                this.FillSpaces(text.Length, 12);
                for (var i = 0 ; i < length ; i++) {
                    this.console.Write("=");
                }
                this.console.WriteLine();
            }
        }


        private void FillSpaces(int length, int total = 15)
        {
            for (var i = length ; i < total ; i++) {
                this.console.Write(" ");
            }
        }
    }
}