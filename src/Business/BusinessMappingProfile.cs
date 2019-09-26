using AutoMapper;
using Lionize.IntegrationMessages;
using TIKSN.Lionize.TaskManagementService.Data.Entities;

namespace TIKSN.Lionize.TaskManagementService.Business
{
    public class BusinessMappingProfile : Profile
    {
        public BusinessMappingProfile()
        {
            CreateMap<TaskUpserted, UserTaskEntity>()
                .ReverseMap();
        }
    }
}