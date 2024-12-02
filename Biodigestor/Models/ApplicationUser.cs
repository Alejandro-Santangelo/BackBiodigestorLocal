using Microsoft.AspNetCore.Identity;

namespace Biodigestor.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool AcceptedTerms { get; set; }
    }
}







