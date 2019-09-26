using Lionize.IntegrationMessages;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Handlers;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

namespace TIKSN.Lionize.TaskManagementService.Business.MessageHandlers
{
    public class TaskUpsertedConsumerMessageHandler : IConsumerMessageHandler<TaskUpserted>
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public TaskUpsertedConsumerMessageHandler(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository ?? throw new ArgumentNullException(nameof(userTaskRepository));
        }

        public async Task HandleAsync(TaskUpserted message, CorrelationID correlationID, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
