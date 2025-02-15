using MultiTenantTaskManager.Interfaces;

namespace MultiTenantTaskManager.Services
{
    public class TenantService : ITenantService
    {
        public Guid CurrentTenantId { get; private set; }

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext?.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdValue) == true)
            {
                if (Guid.TryParse(tenantIdValue, out var tenantGuid))
                {
                    CurrentTenantId = tenantGuid;
                    return;
                }
            }
            
            CurrentTenantId = Guid.Empty;
        }
    }
}
