using AutoMapper;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<UserTaskEntity, MatrixTaskEntity>(MemberList.Source);
        }
    }
}
