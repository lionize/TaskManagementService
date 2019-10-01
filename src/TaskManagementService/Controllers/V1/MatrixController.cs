using AutoMapper;
using Lionize.TaskManagement.ApiModels.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Business.Services;

namespace TIKSN.Lionize.TaskManagementService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class MatrixController : ControllerBase
    {
        private readonly IMatrixTaskService _matrixTaskService;
        private readonly IMapper _mapper;

        public MatrixController(IMatrixTaskService matrixTaskService, IMapper mapper)
        {
            _matrixTaskService = matrixTaskService ?? throw new ArgumentNullException(nameof(matrixTaskService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<MatrixTask[]> GetMatrix(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("sub").Value);

            var entities = await _matrixTaskService.GetActiveAsync(userId, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<MatrixTask[]>(entities);
        }

        [HttpGet("Backlog")]
        public async Task<BacklogTask[]> GetBacklog(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst("sub").Value);

            var entities = await _matrixTaskService.GetBacklogAsync(userId, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<BacklogTask[]>(entities);
        }
    }
}
