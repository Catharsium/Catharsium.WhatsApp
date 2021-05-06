namespace Catharsium.WhatsApp.Entities.Models
{
    public class User
    {
        public string PhoneNumber { get; set; }
        public string NickName { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool IsUnknown { get; set; }


        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(this.DisplayName)
                ? this.DisplayName
                : this.PhoneNumber;
        }
    }
}