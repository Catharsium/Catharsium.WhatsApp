namespace Catharsium.WhatsApp.Entities.Models;

public class Message
{
    public DateTime Timestamp { get; set; }
    public string Sender { get; set; }
    public string Text { get; set; }


    public override string ToString()
    {
        return $"{this.Timestamp:yyyy-MM-dd HH:mm:ss}: {this.Text}";
    }
}