using Lionize.TaskManagement.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPost("SignIn")]
        public async Task<SignInResponse> SignIn([FromBody]SignInRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}