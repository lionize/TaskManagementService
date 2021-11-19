using Microsoft.Extensions.Configuration;
using TIKSN.Data.Mongo;

namespace TIKSN.Lionize.TaskManagementService.Data
{
    public class DatabaseProvider : MongoDatabaseProviderBase
    {
        public DatabaseProvider(
            IMongoClientProvider mongoClientProvider,
            IConfigurationRoot configuration) : base(mongoClientProvider, configuration, "Mongo")
        {
        }
    }
}
