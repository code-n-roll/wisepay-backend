using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Avatars;
using WisePay.Web.Debts.Models;

namespace WisePay.Web.Debts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserWithDebtRaw, UserWithDebt>()
                .ForMember(u => u.AvatarUrl, opt => opt.ResolveUsing<AvatarUrlResolver>())
                .ForMember(u => u.Debt, opt => opt.MapFrom(u => u.Debt))
                .ForMember(u => u.Id, opt => opt.MapFrom(u => u.User.Id))
                .ForMember(u => u.Username, opt => opt.MapFrom(u => u.User.UserName));
        }
    }

    public class AvatarUrlResolver : IValueResolver<UserWithDebtRaw, object, string>
    {
        private readonly AvatarsService _avatarsService;

        public AvatarUrlResolver(AvatarsService avatarsService)
        {
            _avatarsService = avatarsService;
        }

        public string Resolve(UserWithDebtRaw source, object destination, string destMember, ResolutionContext context)
        {
            return _avatarsService.GetFullAvatarUrl(source.User.AvatarPath);
        }
    }
}
