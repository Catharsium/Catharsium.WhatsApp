namespace Catharsium.WhatsApp.Terminal.Models;

public class User
{
    public string PhoneNumber { get; set; }
    public string DisplayName { get; set; }
    public List<string> Aliases { get; set; } = new List<string>();
    public List<string> Conversations { get; set; } = new List<string>();


    public User() { }


    public User(User user)
    {
        this.PhoneNumber = user.PhoneNumber;
        this.DisplayName = user.DisplayName;
        this.Aliases.AddRange(user.Aliases);
        this.Conversations.AddRange(user.Conversations);
    }


    public override string ToString()
    {
        return !string.IsNullOrWhiteSpace(this.DisplayName)
            ? this.DisplayName
            : !string.IsNullOrWhiteSpace(this.PhoneNumber)
                ? this.PhoneNumber
                : this.Aliases.FirstOrDefault();
    }
}