using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MultiTenantTaskManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public Guid TenantId { get; set; }
    }
}
