using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Repository;
using Catharsium.WhatsApp.Entities.Data;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class ImportActiveUsersActionHandler : IActionHandler
    {
        private readonly IWhatsAppRepository whatsAppRespository;
        private readonly IUsersRepository usersRepository;

        public string FriendlyName => "Import active users";

        public Dictionary<string, string> UserData => new Dictionary<string, string> {
            { "BE 2.0", "Bart, Corina, Cynthia, Marina, Mel, Mohanna, Robbert, Sander, +31 6 10451724, +31 6 10769335, +31 6 13654469, +31 6 14474289, +31 6 19802163, +31 6 21358736, +31 6 21812372, +31 6 25131260, +31 6 30193135, +31 6 36035363, +31 6 38494833, +31 6 46093722, +31 6 48106081, +31 6 48200299, +31 6 48225977, +31 6 48424846, +31 6 51618623, +31 6 54341034, +32 471 84 36 05, +32 476 28 75 29, +32 485 54 78 79, +32 485 95 20 28, You" }
        };


        public ImportActiveUsersActionHandler(IWhatsAppRepository whatsAppRespository, IUsersRepository usersRepository)
        {
            this.whatsAppRespository = whatsAppRespository;
            this.usersRepository = usersRepository;
        }


        public async Task Run()
        {
            var conversations = this.whatsAppRespository.GetFiles().Select(f => f.ExtensionlessName);
            foreach (var conversation in conversations) {
                if (!this.UserData.ContainsKey(conversation)) {
                    continue;
                }

                var list = this.UserData[conversation];
                var users = list.Split(", ").Select(u => new User {
                    Aliases = u.StartsWith('+') ? new List<string>() : new List<string> { u },
                    PhoneNumber = u.StartsWith('+') ? u : ""
                });

                await this.usersRepository.UpdateTo(users, conversation);
            }
        }
    }
}