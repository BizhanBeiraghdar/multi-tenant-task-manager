namespace MultiTenantTaskManager.Interfaces
{
    public interface ITenantService
    {
        Guid CurrentTenantId { get; }
    }
}
