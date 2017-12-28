using AutoMapper;
using WisePay.Entities;

namespace WisePay.Web.Avatars
{
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
