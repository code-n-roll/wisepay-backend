using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WisePay.Entities;

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
