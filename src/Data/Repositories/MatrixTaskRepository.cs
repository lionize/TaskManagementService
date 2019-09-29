using MongoDB.Driver;
using System;
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

        public async Task<int> GetMaxOrderOrDefaultAsync(Guid userID, CancellationToken cancellationToken)
        {
            var orders = await collection.Find(Builders<MatrixTaskEntity>.Filter.Eq(f => f.UserID, userID)).Project(entity => entity.Order).ToListAsync(cancellationToken).ConfigureAwait(false);

            if (orders.Count == 0)
                return default;

            return orders.Max();
        }
    }
}
