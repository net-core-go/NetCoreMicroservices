using Microsoft.EntityFrameworkCore;

namespace MS.Identity.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users{get;set;}
        public DbSet<Claim> Claims{get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(user=>{
                user.HasMany(p=>p.Claims)
                    .WithOne(p=>p.User)
                    .HasForeignKey(p=>p.UserId);
            });


            // Customize the ASP.NET< Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}