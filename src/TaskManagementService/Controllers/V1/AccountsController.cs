using Lionize.TaskManagement.ApiModels.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Services;

namespace TaskManagementService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountsController(IAccountService accountService)
        {
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpPost("Refresh")]
        public Task<RefreshTokenResponse> Refresh([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            return accountService.RefreshAsync(request.RefreshToken, cancellationToken);
        }

        [HttpPost("SignIn")]
        public Task<SignInResponse> SignIn([FromBody]SignInRequest request, CancellationToken cancellationToken)
        {
            return accountService.SignInAsync(request.Username, request.Password, cancellationToken);
        }

        [HttpPost("SignOut")]
        public Task<SignOutResponse> SignOut([FromBody]SignOutRequest request, CancellationToken cancellationToken)
        {
            return accountService.SignOutAsync(request.AccessToken, request.RefreshToken, cancellationToken);
        }
    }
}