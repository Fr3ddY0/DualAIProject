using System.ComponentModel.DataAnnotations;

namespace DualAI.Models
{
    public class Mainpage
    {
        [Key]
        public string? Nickname { get; set; }

        public string? Prompt { get; set; }
        public string? LinguaOriginale { get; set; }
        public string? LinguaDiTraduzione { get; set; }
        public string? GeneratedText { get; set; }
        public string? AudioUrl { get; set; }

    }
}
