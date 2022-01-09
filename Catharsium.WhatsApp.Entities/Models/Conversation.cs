namespace Catharsium.WhatsApp.Entities.Models;

public class Conversation
{
    private string _name;
    public string Name
    {
        get {
            return _name;
        }
        set {
            this._name = value.Replace("WhatsApp Chat with ", "");
        }
    }

    public List<Message> Messages { get; set; } = new List<Message>();


    public override string ToString()
    {
        return $"{this.Name} ({this.Messages.Count} messages)";
    }
}