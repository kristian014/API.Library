using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? ObjectId { get; set; }
    }
}
