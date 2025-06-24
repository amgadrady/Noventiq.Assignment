using Microsoft.AspNetCore.Identity;

namespace NoventiqAssignment.DB.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
