using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Data.Entities;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;

namespace TIKSN.Lionize.TaskManagementService.Business.Services
{
    public class MatrixTaskOrderingService : IMatrixTaskOrderingService
    {
        private readonly IMatrixTaskRepository _matrixTaskRepository;

        public MatrixTaskOrderingService(IMatrixTaskRepository matrixTaskRepository)
        {
            _matrixTaskRepository = matrixTaskRepository ?? throw new ArgumentNullException(nameof(matrixTaskRepository));
        }

        public async Task MoveToBacklog(string id, int order, Guid userID, CancellationToken cancellationToken)
        {
            var matrixTask = await _matrixTaskRepository.GetAsync(id, cancellationToken);
            var backlogTasks = await _matrixTaskRepository.GetBacklogTasksAsync(userID, cancellationToken);

            matrixTask.Important = null;
            matrixTask.Urgent = null;

            await InsertMatrixTaskAsync(matrixTask, order, backlogTasks, cancellationToken);
        }

        public async Task MoveToMatrix(string id, bool important, bool urgent, int order, Guid userID, CancellationToken cancellationToken)
        {
            var matrixTask = await _matrixTaskRepository.GetAsync(id, cancellationToken);
            var quadrantTasks = await _matrixTaskRepository.GetMatrixQuadrantAsync(userID, important, urgent, cancellationToken);

            matrixTask.Important = important;
            matrixTask.Urgent = urgent;

            await InsertMatrixTaskAsync(matrixTask, order, quadrantTasks, cancellationToken);
        }

        private async Task InsertMatrixTaskAsync(
            MatrixTaskEntity matrixTask,
            int order,
            IEnumerable<MatrixTaskEntity> sectionTasks,
            CancellationToken cancellationToken)
        {
            if (sectionTasks.Any(x => x.Order == order))
            {
                var toReorder = sectionTasks
                    .Where(x => x.Order >= order)
                    .OrderBy(x => order)
                    .ToArray();

                toReorder = toReorder
                    .TakeWhile((x, i) => i == 0 ? true : (x.Order - toReorder[i - 1].Order) <= 1)
                    .ToArray();

                toReorder
                    .ForEach(x => x.Order++);

                await _matrixTaskRepository.UpdateRangeAsync(toReorder, cancellationToken).ConfigureAwait(false);
            }

            matrixTask.Order = order;

            await _matrixTaskRepository.UpdateAsync(matrixTask, cancellationToken).ConfigureAwait(false);
        }
    }
}