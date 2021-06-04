using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Models
{
    public class User
    {
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public List<string> Aliases { get; set; } = new List<string>();


        public User() { }


        public User(User user)
        {
            this.PhoneNumber = user.PhoneNumber;
            this.DisplayName = user.DisplayName;
            this.IsActive = user.IsActive;
            this.Aliases.AddRange(user.Aliases);
        }


        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(this.DisplayName)
                ? this.DisplayName
                : this.PhoneNumber;
        }
    }
}