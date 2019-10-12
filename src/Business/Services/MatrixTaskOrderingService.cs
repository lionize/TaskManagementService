using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public class MatrixTaskOrderingService : IMatrixTaskOrderingService
    {
        private readonly IMatrixTaskRepository _matrixTaskRepository;

        public MatrixTaskOrderingService(IMatrixTaskRepository matrixTaskRepository)
        {
            this._matrixTaskRepository = matrixTaskRepository ?? throw new ArgumentNullException(nameof(matrixTaskRepository));
        }

        public Task MoveToBacklog(string id, int order, Guid userID, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task MoveToMatrix(string id, bool important, bool urgent, int order, Guid userID, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
