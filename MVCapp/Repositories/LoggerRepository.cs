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
        public async Task AddLogger(Logger logger)
        {
            applicationContext.Loggers.Add(logger);
            await applicationContext.SaveChangesAsync();
        }
    }
}
