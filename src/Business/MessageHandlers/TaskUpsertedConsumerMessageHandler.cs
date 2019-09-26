using Lionize.IntegrationMessages;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Handlers;

namespace TIKSN.Lionize.TaskManagementService.Business.MessageHandlers
{
    public class TaskUpsertedConsumerMessageHandler : IConsumerMessageHandler<TaskUpserted>
    {
        public Task HandleAsync(TaskUpserted message, CorrelationID correlationID, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
