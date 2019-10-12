using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public class MatrixTaskOrderingService : IMatrixTaskOrderingService
    {
        public Task MoveToBacklog(string id, int order, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task MoveToMatrix(string id, bool important, bool urgent, int order, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
