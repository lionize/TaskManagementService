﻿using MediatR;
using System;

namespace TIKSN.Lionize.TaskManagementService.Business.Notifications
{
    /// <summary>
    /// Notifies user task created or updated
    /// </summary>
    public class TaskChangedNotification : INotification
    {
        public TaskChangedNotification(string taskID)
        {
            TaskID = taskID ?? throw new ArgumentNullException(nameof(taskID));
        }

        public string TaskID { get; }
    }
}