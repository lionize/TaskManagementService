using IdentityModel.Client;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Services
{
    public class AccountService
    {
        public async Task SignInAsync(string username, string password, CancellationToken cancellationToken)
        {
            var discoveryClient = new DiscoveryClient("http://localhost:8081");
            var discoveryResponse = await discoveryClient.GetAsync(cancellationToken);
        }
    }
}