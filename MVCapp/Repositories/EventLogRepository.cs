using Microsoft.Identity.Client;
using MVCapp.Migrations;
using MVCapp.Models;
using System.Net;

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
                IPAddress = await GetUserIPAddress(),
                User = user
            };
            applicationContext.EventLogs.Add(eventLog);
            await applicationContext.SaveChangesAsync();
        }
        public async Task<string> GetUserIPAddress()
        {
			string IPAddress = "";
			IPHostEntry Host = default(IPHostEntry);
			string Hostname = null;
			Hostname = System.Environment.MachineName;
			Host = Dns.GetHostEntry(Hostname);
			foreach (IPAddress IP in Host.AddressList)
			{
				if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					IPAddress = Convert.ToString(IP);
				}
			}
            return IPAddress;
		}
    }
}
