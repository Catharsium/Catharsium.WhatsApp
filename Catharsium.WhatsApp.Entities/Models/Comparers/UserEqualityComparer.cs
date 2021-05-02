using System.Collections.Generic;

namespace Catharsium.WhatsApp.Entities.Models.Comparers
{
    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            if (x == null || y == null) {
                return false;
            }

            return x.PhoneNumber == y.PhoneNumber;
        }


        public int GetHashCode(User obj)
        {
            return obj.GetHashCode();
        }
    }
}