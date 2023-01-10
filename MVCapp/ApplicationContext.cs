using Microsoft.EntityFrameworkCore;
using MVCapp.Models;

namespace ProMVC
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
