using System.Linq;
using System.Threading.Tasks;
using MS.Identity.Data;

namespace MS.Identity.Servers
{
    public class UserStore : IUserStore
    {
        private readonly ApplicationDbContext _context;
        public UserStore(ApplicationDbContext context)
        {
            _context=context;
        }
        public async Task<User> FindBySubjectId(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user;
        }

        public async Task<User> FindByUsername(string userName)
        {
            var user = _context.Users.AsQueryable<User>().FirstOrDefault(o=>o.Name==userName);
            return await Task.FromResult(user);
        }

        public bool ValidateCredentials(string userId, string password)
        {
            return _context.Users.Any(o=>o.UserId==userId && o.Password==password);
        }
    }
}