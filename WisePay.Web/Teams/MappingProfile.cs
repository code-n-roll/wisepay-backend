using System.Linq;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Users;

namespace WisePay.Web.Teams
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamShortInfoViewModel>();
            CreateMap<Team, TeamViewModel>()
                .ForMember(t => t.Users, opt => opt.MapFrom(t => t.UserTeams.Select(ut => ut.User)));

            // CreateMap<UserTeam, UserViewModel>()
            //     .ForMember(u => u.Id, opt => opt.MapFrom(ut => ut.User.Id))
            //     .ForMember(u => u.Username, opt => opt.MapFrom(ut => ut.User.Id))
        }
    }
}