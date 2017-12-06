using AutoMapper;
using WisePay.Entities;

namespace WisePay.Web.Teams
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamShortInfoViewModel>();
            CreateMap<Team, TeamViewModel>();
        }
    }
}