using Microsoft.EntityFrameworkCore;
using MVCapp.Models;

namespace MVCapp
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
