using Lionize.TaskManagement.RealtimeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Business.Services;

namespace TIKSN.Lionize.TaskManagementService.Hubs
{
    [Authorize]
    public class MatrixHub : Hub<IMatrixHubClient>
    {
        private readonly IMatrixTaskOrderingService _matrixTaskOrderingService;

        public MatrixHub(IMatrixTaskOrderingService matrixTaskOrderingService)
        {
            _matrixTaskOrderingService = matrixTaskOrderingService ?? throw new ArgumentNullException(nameof(matrixTaskOrderingService));
        }

        public Task MoveToBacklog(MoveToBacklogRequest request)
        {
            return _matrixTaskOrderingService.MoveToBacklog(
                request.TaskId,
                request.Order,
                Context.ConnectionAborted);
        }

        public Task MoveToMatrix(MoveToMatrixRequest request)
        {
            return _matrixTaskOrderingService.MoveToMatrix(
                request.TaskId,
                request.Important,
                request.Urgent,
                request.Order,
                Context.ConnectionAborted);
        }
    }
}