using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class MultiFactorAuthenticator
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Code { get; set; }

        public string QRCodeUrl { get; set; }
    }
}
