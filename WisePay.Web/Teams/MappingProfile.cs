using System.Linq;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Teams.Models;

namespace WisePay.Web.Teams
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamPreview>();
            CreateMap<Team, TeamViewModel>()
                .ForMember(t => t.Users, opt => opt.MapFrom(t => t.UserTeams.Select(ut => ut.User)));
        }
    }
}
