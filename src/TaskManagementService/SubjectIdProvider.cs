using Microsoft.AspNetCore.SignalR;

namespace TIKSN.Lionize.TaskManagementService
{
    public class SubjectIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("sub")?.Value;
        }
    }
}