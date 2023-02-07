using Microsoft.EntityFrameworkCore;

namespace MVCapp.Models
{
    [Keyless]
    public class PhotoModel
    {
        public string? PhotoName { get; set; }
        public string? Path { get; set; }
        public List<Photo> PhotoList { get; set; }
    }
}
