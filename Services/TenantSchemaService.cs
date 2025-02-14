using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Data;

namespace MultiTenantTaskManager.Services
{
    public class TenantSchemaService
    {
        private readonly ApplicationDbContext _context;

        public TenantSchemaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void EnsureSchemaExists(string schemaName)
        {
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                Console.WriteLine($"Ensuring schema exists: {schemaName}");
                _context.Database.ExecuteSqlRaw($"CREATE SCHEMA IF NOT EXISTS \"{schemaName}\"");
            }
        }
    }
}
