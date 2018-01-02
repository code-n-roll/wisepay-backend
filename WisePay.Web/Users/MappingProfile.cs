using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Avatars;
using WisePay.Web.Users.Models;

namespace WisePay.Web.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CurrentUserViewModel>()
                .ForMember(u => u.AvatarUrl, opt => opt.ResolveUsing<AvatarUrlResolver>());
            CreateMap<User, UserViewModel>()
                .ForMember(u => u.AvatarUrl, opt => opt.ResolveUsing<AvatarUrlResolver>());
        }
    }

    public class AvatarUrlResolver : IValueResolver<User, object, string>
    {
        private readonly AvatarsService _avatarsService;

        public AvatarUrlResolver(AvatarsService avatarsService)
        {
            _avatarsService = avatarsService;
        }

        public string Resolve(User source, object destination, string destMember, ResolutionContext context)
        {
            return _avatarsService.GetFullAvatarUrl(source.AvatarPath);
        }
    }
}
