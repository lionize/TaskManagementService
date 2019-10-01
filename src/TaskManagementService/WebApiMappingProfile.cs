using AutoMapper;
using Lionize.TaskManagement.ApiModels.V1;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService
{
    public class WebApiMappingProfile : Profile
    {
        public WebApiMappingProfile()
        {
            CreateMap<MatrixTaskEntity, BacklogTask>(MemberList.Destination);
            CreateMap<MatrixTaskEntity, MatrixTask>(MemberList.Destination);
        }
    }
}