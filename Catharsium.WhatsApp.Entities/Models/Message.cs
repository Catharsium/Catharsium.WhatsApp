using System;

namespace Catharsium.WhatsApp.Entities.Models
{
    public class Message
    {
        public DateTime Date { get; set; }
        public User Sender { get; set; }
        public string Text { get; set; }
    }
}