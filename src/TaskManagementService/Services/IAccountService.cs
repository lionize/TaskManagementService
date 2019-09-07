using Lionize.TaskManagement.ApiModels.V1;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Services
{
    public interface IAccountService
    {
        Task<SignInResponse> SignInAsync(string username, string password, CancellationToken cancellationToken);
        Task<SignOutResponse> SignOutAsync(string accessToken, string refreshToken, CancellationToken cancellationToken);
        Task<RefreshTokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
    }
}