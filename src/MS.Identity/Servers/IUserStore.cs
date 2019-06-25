using System.Threading.Tasks;
using MS.Identity.Data;
using System.Collections;
using System.Collections.Generic;

namespace MS.Identity.Servers
{
    public interface IUserStore
    {
        Task<User> FindBySubjectId(string userId);
        Task<User> FindByUsername(string userName);
        bool ValidateCredentials(string userId,string password);
    }
}