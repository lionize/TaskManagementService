using Lionize.TaskManagement.ApiModels.V1;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Hubs
{
    public interface IMatrixHubClient
    {
        Task MovedToBacklog(BacklogTask payload);

        Task MovedToMatrix(MatrixTask payload);
    }
}