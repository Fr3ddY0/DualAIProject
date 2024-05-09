using System.ComponentModel.DataAnnotations;

namespace DualAI.Models
{
    public class RedirectViewModel
    {
        [Key]
        public int id { get; set; }
        public string GeneratedText { get; set; }
        public string AudioUrl { get; set; }
    }
}
