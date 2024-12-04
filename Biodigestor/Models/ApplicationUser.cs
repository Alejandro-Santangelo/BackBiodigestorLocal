using Microsoft.AspNetCore.Identity;

namespace Biodigestor.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool AcceptedTerms { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? ProfilePictureContentType { get; set; }
    }
}
