using Catharsium.Util.IO.Console.Interfaces;
using Catharsium.WhatsApp.Data.Repository;
using Catharsium.WhatsApp.Entities.Data;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Terminal.ActionHandlers
{
    public class ImportActiveUsersActionHandler : IActionHandler
    {
        private readonly IActiveUsersRepository activeUsersRepository;
        private readonly IConversationsRepository whatsAppRespository;
        private readonly IConversationUsersRepository conversationUsersRepository;

        public string FriendlyName => "Import active users";


        public ImportActiveUsersActionHandler(IActiveUsersRepository activeUsersRepository, IConversationsRepository whatsAppRespository, IConversationUsersRepository conversationUsersRepository)
        {
            this.activeUsersRepository = activeUsersRepository;
            this.whatsAppRespository = whatsAppRespository;
            this.conversationUsersRepository = conversationUsersRepository;
        }


        public async Task Run()
        {
            var conversations = await this.whatsAppRespository.GetConversations();
            foreach (var conversation in conversations) {
                var conversationUsers = this.activeUsersRepository.GetFor(conversation.Name);
                await this.conversationUsersRepository.Update(conversationUsers, conversation.Name);
            }
        }
    }
}