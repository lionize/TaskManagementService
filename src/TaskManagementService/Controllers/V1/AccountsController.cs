using Lionize.TaskManagement.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Services;

namespace TaskManagementService.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountsController(IAccountService accountService)
        {
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpPost("SignIn")]
        public Task<SignInResponse> SignIn([FromBody]SignInRequest request, CancellationToken cancellationToken)
        {
            return accountService.SignInAsync(request.Username, request.Password, cancellationToken);
        }
    }
}