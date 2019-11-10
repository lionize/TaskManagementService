using MediatR;
using System;

namespace TIKSN.Lionize.TaskManagementService.Business.Notifications
{
    /// <summary>
    /// Notifies about matrix task movement
    /// </summary>
    public class TaskMovedNotification : INotification
    {
        public TaskMovedNotification(string taskID)
        {
            TaskID = taskID ?? throw new ArgumentNullException(nameof(taskID));
        }

        public string TaskID { get; }
    }
}