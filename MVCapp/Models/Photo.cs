using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public string? PhotoName { get; set; }
        //[Display(Name = "")]
        public string? Path { get; set; }
    }
}
