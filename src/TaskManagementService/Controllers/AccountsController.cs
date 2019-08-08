using Lionize.TaskManagement.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Validators;

namespace TaskManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpPost("SignIn")]
        public async Task<SignInResponse> SignIn([FromBody]SignInRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("SignUp")]
        public async Task<SignUpResponse> SignUp([FromBody]SignUpRequest request)
        {
            var validator = new SignUpRequestValidator();

            var validation = validator.Validate(request);

            if (validation.IsValid)
            {
                throw new NotImplementedException();
            }
            else
            {
                return new SignUpResponse { IsError = true, ErrorMessage = validation.Errors.First().ErrorMessage };
            }
        }
    }
}