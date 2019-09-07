using System;

namespace TIKSN.Lionize.TaskManagementService.Business
{
    public interface IEndpointAddressProvider
    {
        Uri GetEndpointAddress(string queueName);
    }
}