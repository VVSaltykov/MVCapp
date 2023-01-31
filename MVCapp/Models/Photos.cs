using System.ComponentModel.DataAnnotations;

namespace MVCapp.Models
{
    public class Photos
    {
        [Key]
        public int PhotoId { get; set; }
        public string? PhotoName { get; set; }
        public byte[]? Photo { get; set; }
        public string? Path { get; set; }
    }
}
