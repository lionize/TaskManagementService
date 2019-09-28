using TIKSN.Data.Mongo;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Data.Repositories
{
    public interface IUserTaskRepository : IMongoRepository<UserTaskEntity, string>
    {
    }
}