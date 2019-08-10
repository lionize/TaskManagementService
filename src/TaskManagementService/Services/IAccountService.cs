using System.Threading;
using System.Threading.Tasks;
using Lionize.TaskManagement.ApiModels;

namespace TIKSN.Lionize.TaskManagementService.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(string username, string password, CancellationToken cancellationToken);
    }
}