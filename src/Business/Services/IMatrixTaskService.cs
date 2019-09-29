using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public interface IMatrixTaskService
    {
        Task<MatrixTaskEntity[]> GetActiveAsync(Guid userId, CancellationToken cancellationToken);
        Task<MatrixTaskEntity[]> GetBacklogAsync(Guid userId, CancellationToken cancellationToken);
    }
}