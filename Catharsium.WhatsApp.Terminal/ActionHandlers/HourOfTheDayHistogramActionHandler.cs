using Catharsium.Math.Graph.Interfaces;
using Catharsium.Math.Graph.Models;
using Catharsium.Util.Filters;
using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Filters;
using Catharsium.WhatsApp.Entities.Models;
using Catharsium.WhatsApp.Entities.Terminal.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class HourOfTheDayHistogramActionHandler : IActionHandler
    {
        private readonly IConversationChooser conversationChooser;
        private readonly IPeriodChooser periodChooser;
        private readonly IEqualityComparer<User> userComparer;
        private readonly IGraph graph;
        private readonly IConsole console;

        public string FriendlyName => "Hour of the Day Histogram";


        public HourOfTheDayHistogramActionHandler(
            IConversationChooser conversationChooser,
            IPeriodChooser periodChooser,
            IEqualityComparer<User> userComparer,
            IGraph graph,
            IConsole console)
        {
            this.conversationChooser = conversationChooser;
            this.periodChooser = periodChooser;
            this.userComparer = userComparer;
            this.graph = graph;
            this.console = console;
        }


        public async Task Run()
        {
            var period = await this.periodChooser.AskForPeriod();
            var messages = await this.conversationChooser.AskAndLoad();
            messages = messages.Include(new PeriodFilter(period));

            var users = messages.Select(m => m.Sender).Distinct(this.userComparer);
            var user = this.console.AskForItem(users);
            if (user != null) {
                messages = messages.Include(new UserFilter(user));
            }

            var groups = messages.GroupBy(m => m.Timestamp.Hour).Select(g => new { g.First().Timestamp.Hour, Messages = g.Count() }).OrderBy(g => g.Hour);
            var graphData = new Dictionary<string, decimal>();
            foreach (var group in groups) {
                graphData.Add(group.Hour.ToString(), group.Messages);
            }
            for (var i = 0 ; i < 24 ; i++) {
                if (!graphData.ContainsKey(i.ToString())) {
                    graphData[i.ToString()] = 0;
                }
            }

            var fromValue = period.From != DateTime.MinValue ? $"{period.From:dd-MM-yyyy}" : "*";
            var toValue = period.To != DateTime.MaxValue ? $"{period.To:dd-MM-yyyy}" : "*";

            this.console.WriteLine($"Last update:\t{messages.Last().Timestamp:dd-MM-yyyy (HH:mm)}");
            if (user != null) {
                this.console.WriteLine($"User:\t{user}");
            }
            this.console.WriteLine($"Period:\t\t{fromValue} - {toValue}");
            this.console.WriteLine($"Message:\t{messages.Count()}");

            this.graph.Generate(new GraphData { Data = graphData.AsEnumerable().OrderBy(d => int.Parse(d.Key)), Size = 100 });
        }
    }
}