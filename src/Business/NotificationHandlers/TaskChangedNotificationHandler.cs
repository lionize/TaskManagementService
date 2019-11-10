using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Business.Notifications;
using TIKSN.Lionize.TaskManagementService.Data.Entities;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

namespace TIKSN.Lionize.TaskManagementService.Business.NotificationHandlers
{
    public class TaskChangedNotificationHandler : INotificationHandler<TaskChangedNotification>
    {
        private readonly IMapper _mapper;
        private readonly IMatrixTaskRepository _matrixTaskRepository;
        private readonly IMediator _mediator;
        private readonly IUserTaskRepository _userTaskRepository;

        public TaskChangedNotificationHandler(IMatrixTaskRepository matrixTaskRepository, IUserTaskRepository userTaskRepository, IMapper mapper, IMediator mediator)
        {
            _matrixTaskRepository = matrixTaskRepository ?? throw new ArgumentNullException(nameof(matrixTaskRepository));
            _userTaskRepository = userTaskRepository ?? throw new ArgumentNullException(nameof(userTaskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(TaskChangedNotification notification, CancellationToken cancellationToken)
        {
            var userTask = await _userTaskRepository.GetAsync(notification.TaskID, cancellationToken).ConfigureAwait(false);
            var matrixTask = await _matrixTaskRepository.GetOrDefaultAsync(userTask.ID, cancellationToken).ConfigureAwait(false);

            if (matrixTask == null)
            {
                matrixTask = _mapper.Map<MatrixTaskEntity>(userTask);
                matrixTask.Order = await _matrixTaskRepository.GetMaxOrderOrDefaultAsync(userTask.UserID, cancellationToken).ConfigureAwait(false) + 1;
                await _matrixTaskRepository.AddAsync(matrixTask, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                matrixTask = _mapper.Map(userTask, matrixTask);
                await _matrixTaskRepository.UpdateAsync(matrixTask, cancellationToken).ConfigureAwait(false);
            }

            await _mediator.Publish(new TaskMovedNotification(matrixTask.ID)).ConfigureAwait(false);
        }
    }
}