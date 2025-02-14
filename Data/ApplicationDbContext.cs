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
            _tenantSchema = httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Schema"].ToString() ?? "public";

            // Ensure the schema exists before using it
            using var scope = httpContextAccessor.HttpContext?.RequestServices.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext?.Database.ExecuteSqlRaw($"CREATE SCHEMA IF NOT EXISTS \"{_tenantSchema}\"");
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
