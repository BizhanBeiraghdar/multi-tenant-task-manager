using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Data;
using MultiTenantTaskManager.Models;

namespace MultiTenantTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TenantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOK()
        {
            return Ok(new { Message = "Tenant registered successfully" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTenant([FromBody] Tenant tenantRequest)
        {
            if (string.IsNullOrWhiteSpace(tenantRequest.Name))
            {
                return BadRequest("Tenant name is required.");
            }

            // Generate schema name dynamically
            string schemaName = $"tenant_{tenantRequest.Name.ToLower().Replace(" ", "_")}";

            // Check if schema already exists
            if (await _context.Tenants.AnyAsync(t => t.DatabaseSchema == schemaName))
            {
                return Conflict("Tenant already exists.");
            }

            // Create new schema for the tenant
            await _context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA IF NOT EXISTS \"{schemaName}\"");

            // Save tenant details in the tenants table
            var newTenant = new Tenant
            {
                Name = tenantRequest.Name,
                DatabaseSchema = schemaName
            };

            _context.Tenants.Add(newTenant);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tenant registered successfully", Schema = schemaName });
        }
    }
}
