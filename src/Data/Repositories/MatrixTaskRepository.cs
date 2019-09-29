using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Data.Repositories
{
    public class MatrixTaskRepository : MongoRepository<MatrixTaskEntity, string>, IMatrixTaskRepository
    {
        public MatrixTaskRepository(IMongoDatabaseProvider mongoDatabaseProvider) : base(mongoDatabaseProvider, "MatrixTasks")
        {
        }

        public Task<IEnumerable<MatrixTaskEntity>> GetActiveAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MatrixTaskEntity>> GetBacklogTasksAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await collection.Find(FilterByUser(userId)).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        private static FilterDefinition<MatrixTaskEntity> FilterByUser(Guid userId)
        {
            return Builders<MatrixTaskEntity>.Filter.Eq(f => f.UserID, userId);
        }

        public async Task<int> GetMaxOrderOrDefaultAsync(Guid userID, CancellationToken cancellationToken)
        {
            var orders = await collection.Find(FilterByUser(userID)).Project(entity => entity.Order).ToListAsync(cancellationToken).ConfigureAwait(false);

            if (orders.Count == 0)
            {
                return default;
            }

            return orders.Max();
        }
    }
}
