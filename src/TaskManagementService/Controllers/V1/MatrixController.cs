using Lionize.TaskManagement.ApiModels.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class MatrixController : ControllerBase
    {
        [HttpGet]
        public async Task<MatrixTask[]> GetMatrix()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Backlog")]
        public async Task<BacklogTask[]> GetBacklog()
        {
            throw new NotImplementedException();
        }
    }
}
