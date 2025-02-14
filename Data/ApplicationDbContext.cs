using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Models;

namespace MultiTenantTaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _tenantSchema;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            // Get tenant schema from request headers (default to 'public' if not provided)
            if (!string.IsNullOrEmpty(httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Schema"].ToString()))
            {
                _tenantSchema = httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Schema"].ToString();
            }
            else
            {
                _tenantSchema = "public";
            }
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().ToTable("tenants", schema: "public");

            // Map TaskItem to a dynamic schema (multi-tenancy)
            modelBuilder.Entity<TaskItem>().ToTable("tasks", schema: _tenantSchema);
        }
    }
}
