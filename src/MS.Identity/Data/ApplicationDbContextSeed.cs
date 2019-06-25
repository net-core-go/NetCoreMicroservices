using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MS.Identity.Data
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context,IHostingEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings,int? retry = 0)
            {
                List<User> defaultUsers=new List<User>();
                if(!context.Users.Any())
                {
                    defaultUsers.Add(new User{ UserId="test0001",Name="yangfan",Password="11111",IsActive=true});
                    defaultUsers.Add(new User{ UserId="test0002",Name="xiaowang",Password="11111",IsActive=true});
                    context.Users.AddRange(defaultUsers);
                }
                
                await context.SaveChangesAsync();
            }
        
    }
}