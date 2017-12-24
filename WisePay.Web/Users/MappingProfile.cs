using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Users.Models;

namespace WisePay.Web.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CurrentUserViewModel>();
            CreateMap<User, UserViewModel>();
        }
    }
}
