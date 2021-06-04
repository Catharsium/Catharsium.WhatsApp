using Catharsium.Util.IO.Interfaces;
using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catharsium.WhatsApp.Data.Repository
{
    public interface IUsersRepository
    {
        IFile GetFile(string fileName);

        Task<IEnumerable<User>> ReadFrom(string fileName);

        Task UpdateTo(IEnumerable<User> users, string fileName);
    }
}