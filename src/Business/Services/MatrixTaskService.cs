using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Data.Entities;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public class MatrixTaskService : IMatrixTaskService
    {
        private readonly IMatrixTaskRepository _matrixTaskRepository;

        public MatrixTaskService(IMatrixTaskRepository matrixTaskRepository)
        {
            _matrixTaskRepository = matrixTaskRepository ?? throw new ArgumentNullException(nameof(matrixTaskRepository));
        }

        public async Task<IEnumerable<MatrixTaskEntity>> GetBacklogAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _matrixTaskRepository.GetBacklogTasksAsync(userId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<MatrixTaskEntity>> GetActiveAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _matrixTaskRepository.GetActiveAsync(userId, cancellationToken).ConfigureAwait(false);
        }
    }
}
