using TIKSN.Data.Mongo;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Data.Repositories
{
    public class UserTaskRepository : MongoRepository<UserTaskEntity, string>, IUserTaskRepository
    {
        public UserTaskRepository(IMongoDatabaseProvider mongoDatabaseProvider) : base(mongoDatabaseProvider, "UserTasks")
        {
        }
    }
}
