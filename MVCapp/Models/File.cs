using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCapp.Models
{
    public class File
    {
        [Key]
        public int FileId { get; set; }
        public string? FileName { get; set; }
        //[Display(Name = "")]
        public string? Path { get; set; }
    }
}
