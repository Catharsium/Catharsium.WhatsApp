using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Data.Repository
{
    public interface IConversationUsersRepository
    {
        Task<List<User>> GetAll(string fileName);

        Task Update(IEnumerable<User> users, string fileName);
    }
}