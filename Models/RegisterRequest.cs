using System.ComponentModel.DataAnnotations;

namespace MultiTenantTaskManager.Models
{
    public class RegisterRequest
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
