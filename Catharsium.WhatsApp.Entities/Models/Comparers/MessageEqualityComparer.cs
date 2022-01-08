namespace Catharsium.WhatsApp.Terminal.Models.Comparers;

public class MessageEqualityComparer : IEqualityComparer<Message>
{
    public bool Equals(Message x, Message y)
    {
        return x != null
            && y != null
            && x.Timestamp == y.Timestamp
            && x.Sender == y.Sender
            && x.Text == y.Text;
    }


    public int GetHashCode(Message obj)
    {
        return obj.Text.GetHashCode();
    }
}