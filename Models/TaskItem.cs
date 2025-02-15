using System.ComponentModel.DataAnnotations;

namespace MultiTenantTaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
