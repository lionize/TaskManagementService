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
            var query = await collection.FindAsync(Builders<MatrixTaskEntity>.Filter.Eq(f => f.UserID, userID), cancellationToken: cancellationToken).ConfigureAwait(false);

            var maxOrder = 0;

            while (await query.MoveNextAsync(cancellationToken).ConfigureAwait(false))
            {
                maxOrder = Math.Max(maxOrder, query.Current.Max(x => x.Order));
            }

            return maxOrder;
        }
    }
}
