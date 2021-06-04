using System;
using System.Linq;

namespace Catharsium.WhatsApp.Entities.Models
{
    public class UserStatistics
    {
        public User User { get; }
        public IOrderedEnumerable<Message> Messages { get; }


        public UserStatistics(User user, IOrderedEnumerable<Message> messages)
        {
            this.User = user;
            this.Messages = messages;
        }

        public Message FirstMessage => this.Messages.First();
        public Message LastMessage => this.Messages.Last();
        public int ActiveDays => (int)Math.Round((this.LastMessage.Timestamp - this.FirstMessage.Timestamp).TotalDays) + 1;
        public double MessagesPerDay => (double)this.Messages.Count() / (double)this.ActiveDays;
        public int TotalMessages => this.Messages.Count();
        public int TotalCharacters => string.Join("", this.Messages.Select(m => m.Text)).Length;
        public double AverageMessageLength => (double)this.TotalCharacters / (double)this.Messages.Count();
    }
}