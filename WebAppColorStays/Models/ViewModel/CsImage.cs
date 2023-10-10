using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppColorStays.Models.ViewModel
{
    public class CsImage
    {
        public string? Id { get; set; }
        public string? FileName { get; set; }
        //[NotMapped]
        //public IFormFile? File { get; set; }
        public string? ImageURL { get; set; }
    }
}
