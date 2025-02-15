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
                return BadRequest("Tenant name is required.");

            // Create a new tenant
            tenantRequest.Id = Guid.NewGuid();
            _context.Tenants.Add(tenantRequest);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tenant registered successfully", TenantId = tenantRequest.Id });
        }
    }
}
