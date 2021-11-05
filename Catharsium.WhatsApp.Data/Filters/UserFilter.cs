using Catharsium.Util.Filters;
using Catharsium.WhatsApp.Entities.Models;

namespace Catharsium.WhatsApp.Data.Filters
{
    public class UserFilter : IFilter<Message>
    {
        private readonly User user;


        public UserFilter(User user)
        {
            this.user = user;
        }


        public bool Includes(Message item)
        {
            return item.Sender.PhoneNumber == this.user.PhoneNumber;
        }
    }
}