using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace Catharsium.WhatsApp.Data.Repository
{
    public class ActiveUsersRepository : IActiveUsersRepository
    {
        private readonly WhatsAppDataSettings settings;


        public ActiveUsersRepository(WhatsAppDataSettings settings)
        {
            this.settings = settings;
        }


        public List<User> GetFor(string conversationName)
        {
            var result = new List<User>();
            if (this.settings.ActiveUsers.ContainsKey(conversationName)) {

                var list = this.settings.ActiveUsers[conversationName];
                result = list.Split(", ").Select(u => new User {
                    Aliases = u.StartsWith('+') ? new List<string>() : new List<string> { u },
                    PhoneNumber = u.StartsWith('+') ? u : ""
                }).ToList();
            }

            return result;
        }
    }
}