using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Users { get; set; }
        public string IPAddress { get; set; }
    }
}
