using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Models.Comparers
{
    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x != null
                && y != null
                && !string.IsNullOrWhiteSpace(x.PhoneNumber)
                && !string.IsNullOrWhiteSpace(y.PhoneNumber)
                && x.PhoneNumber == y.PhoneNumber;
        }


        public int GetHashCode(User obj)
        {
            return obj.PhoneNumber.GetHashCode();
        }
    }
}