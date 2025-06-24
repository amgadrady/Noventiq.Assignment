using Microsoft.AspNetCore.Identity;

namespace NoventiqAssignment.DB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
