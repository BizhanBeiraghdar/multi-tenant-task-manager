using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Interfaces;
using MultiTenantTaskManager.Models;

namespace MultiTenantTaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantService _tenantService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
        }

        // DbSets for our models
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tenant data (the Tenants table) remains in the public schema (no filtering needed)
            modelBuilder.Entity<Tenant>().ToTable("tenants");

            modelBuilder.Entity<TaskItem>(entity =>
            {
                // Tasks will be in the public schema (or if using dynamic multi-tenancy, the default schema).
                entity.ToTable("tasks");

                // Global query filter to ensure each query only returns data for the current tenant.
                entity.HasQueryFilter(t => t.TenantId == _tenantService.CurrentTenantId);

                // Configure TenantId as a foreign key.
                // If Tenant has a navigation property (e.g., public ICollection<TaskItem> Tasks), you can change WithMany().
                entity.HasOne<Tenant>()
                      .WithMany()
                      .HasForeignKey(t => t.TenantId)
                      .IsRequired();
            });
        }
    }
}
