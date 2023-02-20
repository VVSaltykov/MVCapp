using Microsoft.EntityFrameworkCore;
using MVCapp.Models;
using File = MVCapp.Models.File;

namespace MVCapp
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
