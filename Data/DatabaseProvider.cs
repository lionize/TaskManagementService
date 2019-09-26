using Microsoft.Extensions.Configuration;
using TIKSN.Data.Mongo;

namespace TIKSN.Lionize.TaskManagementService.Data
{
    public class DatabaseProvider : MongoDatabaseProvider
    {
        public DatabaseProvider(IConfigurationRoot configuration) : base(configuration, "Mongo")
        {
        }
    }
}
