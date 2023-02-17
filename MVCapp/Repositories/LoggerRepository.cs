using Microsoft.Identity.Client;
using MVCapp.Migrations;
using MVCapp.Models;
using Logger = MVCapp.Models.Logger;

namespace MVCapp.Repositories
{
    public class LoggerRepository
    {
        private readonly ApplicationContext applicationContext;
        
        public LoggerRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        public async Task AddLogger(string information, User user)
        {
            Logger logger = new Logger
            {
                Date = DateTime.Now,
                Information = information,
                User = user
            };
            applicationContext.Loggers.Add(logger);
            await applicationContext.SaveChangesAsync();
        }
    }
}
