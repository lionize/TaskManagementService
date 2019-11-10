using AutoMapper;
using Lionize.TaskManagement.ApiModels.V1;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Business.Notifications;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;
using TIKSN.Lionize.TaskManagementService.Hubs;

namespace TIKSN.Lionize.TaskManagementService.NotificationHandlers
{
    public class TaskMovedNotificationHandler : INotificationHandler<TaskMovedNotification>
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<MatrixHub, IMatrixHubClient> _matrixHubClientContext;
        private readonly IMatrixTaskRepository _matrixTaskRepository;

        public TaskMovedNotificationHandler(IMatrixTaskRepository matrixTaskRepository, IHubContext<MatrixHub, IMatrixHubClient> matrixHubClientContext, IMapper mapper)
        {
            _matrixTaskRepository = matrixTaskRepository ?? throw new ArgumentNullException(nameof(matrixTaskRepository));
            _matrixHubClientContext = matrixHubClientContext ?? throw new ArgumentNullException(nameof(matrixHubClientContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Handle(TaskMovedNotification notification, CancellationToken cancellationToken)
        {
            var matrixTask = await _matrixTaskRepository.GetAsync(notification.TaskID, cancellationToken).ConfigureAwait(false);

            if (matrixTask.Completed)
            {
                //TODO: Notify completed/archived
            }
            else
            {
                var matrixHubClient = _matrixHubClientContext.Clients.User(matrixTask.UserID.ToString());
                if (matrixTask.Important.HasValue && matrixTask.Urgent.HasValue)
                {
                    await matrixHubClient.MovedToMatrix(_mapper.Map<MatrixTask>(matrixTask)).ConfigureAwait(false);
                }
                else
                {
                    await matrixHubClient.MovedToBacklog(_mapper.Map<BacklogTask>(matrixTask)).ConfigureAwait(false);
                }
            }
        }
    }
}