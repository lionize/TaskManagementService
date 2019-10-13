using System;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public interface IMatrixTaskOrderingService
    {
        Task MoveToBacklog(string id, int order, Guid userID, CancellationToken cancellationToken);

        Task MoveToMatrix(string id, bool important, bool urgent, int order, Guid userID, CancellationToken cancellationToken);
    }
}