using Autofac;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

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

            builder
                .RegisterType<UserTaskRepository>()
                .As<IUserTaskRepository>()
                .SingleInstance();

            builder
                .RegisterType<MatrixTaskRepository>()
                .As<IMatrixTaskRepository>()
                .SingleInstance();
        }
    }
}
