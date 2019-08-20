namespace TIKSN.Lionize.TaskManagementService.Options
{
    public class ServiceDiscoveryOptions
    {
        public ServiceInfo Identity { get; set; }

        public class ServiceInfo
        {
            public string BaseAddress { get; set; }
        }
    }
}