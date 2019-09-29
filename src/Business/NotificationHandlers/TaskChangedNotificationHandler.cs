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
        private readonly IMatrixTaskRepository _matrixTaskRepository;
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IMapper _mapper;

        public TaskChangedNotificationHandler(IMatrixTaskRepository matrixTaskRepository, IUserTaskRepository userTaskRepository, IMapper mapper)
        {
            _matrixTaskRepository = matrixTaskRepository ?? throw new ArgumentNullException(nameof(matrixTaskRepository));
            _userTaskRepository = userTaskRepository ?? throw new ArgumentNullException(nameof(userTaskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        }
    }
}
