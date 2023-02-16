using System.Reflection.Metadata;

namespace MVCapp.Models
{
    public class Logger
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Information { get; set; }
        public User? User { get; set; }
    }
}
