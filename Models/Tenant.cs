namespace MultiTenantTaskManager.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DatabaseSchema { get; set; } // Schema for each tenant
    }
}
