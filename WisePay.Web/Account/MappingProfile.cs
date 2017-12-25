using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Account.Models;

namespace WisePay.Web.Account
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, ProfileViewModel>();
        }
    }
}
