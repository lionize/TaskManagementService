using Autofac;
using TIKSN.Data.Mongo;

namespace TIKSN.Lionize.TaskManagementService.Data
{
    public class DataAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DatabaseProvider>()
                .As<IMongoDatabaseProvider>()
                .SingleInstance();
        }
    }
}
