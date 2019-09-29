using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public interface IMatrixTaskService
    {
        Task<IEnumerable<MatrixTaskEntity>> GetActiveAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<MatrixTaskEntity>> GetBacklogAsync(Guid userId, CancellationToken cancellationToken);
    }
}