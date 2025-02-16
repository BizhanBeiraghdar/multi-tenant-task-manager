using MultiTenantTaskManager.Interfaces;

namespace MultiTenantTaskManager.Services
{
    public class TenantService : ITenantService
    {
        private readonly ILogger<TenantService> _logger;
        public Guid CurrentTenantId { get; private set; } = Guid.Empty;

        public TenantService(IHttpContextAccessor httpContextAccessor, ILogger<TenantService> logger)
        {
            _logger = logger;
            var user = httpContextAccessor.HttpContext?.User;

            if (user != null)
            {
                var tenantIdClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId");

                if (tenantIdClaim != null && Guid.TryParse(tenantIdClaim.Value, out Guid tenantGuid))
                {
                    CurrentTenantId = tenantGuid;
                    _logger.LogInformation($"Extracted TenantId from JWT: {CurrentTenantId}");
                }
                else
                {
                    _logger.LogWarning("TenantId claim is missing or invalid.");
                }
            }
            else
            {
                _logger.LogWarning("No user claims found in HTTP context.");
            }
        }
    }
}
