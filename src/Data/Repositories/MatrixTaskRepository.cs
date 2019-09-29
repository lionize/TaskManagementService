using TIKSN.Data.Mongo;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Data.Repositories
{
    public class MatrixTaskRepository : MongoRepository<MatrixTaskEntity, string>, IMatrixTaskRepository
    {
        public MatrixTaskRepository(IMongoDatabaseProvider mongoDatabaseProvider) : base(mongoDatabaseProvider, "MatrixTasks")
        {
        }
    }
}
