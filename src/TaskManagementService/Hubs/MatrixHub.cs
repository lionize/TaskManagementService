using Lionize.TaskManagement.RealtimeModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Hubs
{
    public class MatrixHub : Hub<IMatrixHubClient>
    {
        public Task MoveToMatrix(MoveToMatrixRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task MoveToBacklog(MoveToBacklogRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}