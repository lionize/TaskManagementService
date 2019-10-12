using Lionize.TaskManagement.RealtimeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Hubs
{
    [Authorize]
    public class MatrixHub : Hub<IMatrixHubClient>
    {
        public Task MoveToBacklog(MoveToBacklogRequest request, CancellationToken cancellationToken)
        {
            throw new HubException("Not implemented yet.");
        }

        public Task MoveToMatrix(MoveToMatrixRequest request, CancellationToken cancellationToken)
        {
            throw new HubException("Not implemented yet.");
        }
    }
}