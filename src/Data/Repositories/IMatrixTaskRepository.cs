using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Data.Repositories
{
    public interface IMatrixTaskRepository : IMongoRepository<MatrixTaskEntity, string>
    {
        Task<int> GetMaxOrderOrDefaultAsync(Guid userID, CancellationToken cancellationToken);
        Task<MatrixTaskEntity[]> GetBacklogTasksAsync(Guid userId, CancellationToken cancellationToken);
        Task<MatrixTaskEntity[]> GetActiveAsync(Guid userId, CancellationToken cancellationToken);
    }
}