using AutoMapper;
using Lionize.IntegrationMessages;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Handlers;
using TIKSN.Lionize.TaskManagementService.Business.Notifications;
using TIKSN.Lionize.TaskManagementService.Data.Entities;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

namespace TIKSN.Lionize.TaskManagementService.Business.MessageHandlers
{
    public class TaskUpsertedConsumerMessageHandler : IConsumerMessageHandler<TaskUpserted>
    {
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TaskUpsertedConsumerMessageHandler(IUserTaskRepository userTaskRepository, IMediator mediator, IMapper mapper)
        {
            _userTaskRepository = userTaskRepository ?? throw new ArgumentNullException(nameof(userTaskRepository));
            _mediator = mediator;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task HandleAsync(TaskUpserted message, CorrelationID correlationID, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<UserTaskEntity>(message);
            await _userTaskRepository.AddOrUpdateAsync(entity, cancellationToken).ConfigureAwait(false);
            await _mediator.Publish(new TaskChangedNotification(entity.ID), cancellationToken).ConfigureAwait(false);
        }
    }
}
