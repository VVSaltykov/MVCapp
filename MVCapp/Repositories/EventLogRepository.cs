using Microsoft.Identity.Client;
using MVCapp.Migrations;
using MVCapp.Models;

namespace MVCapp.Repositories
{
    public class EventLogRepository
    {
        private readonly ApplicationContext applicationContext;
        
        public EventLogRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        public async Task AddLogger(string information, User user)
        {
            EventLog eventLog = new EventLog
            {
                Date = DateTime.Now,
                Information = information,
                User = user
            };
            applicationContext.EventLogs.Add(eventLog);
            await applicationContext.SaveChangesAsync();
        }
    }
}
