using System.Linq;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Avatars;
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Purchases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserPurchase, UserPurchaseInfo>()
                .ForMember(up => up.AvatarUrl, opt => opt.ResolveUsing<AvatarUrlResolver>())
                .ForMember(up => up.Username, opt => opt.MapFrom(up => up.User.UserName))
                .ForMember(up => up.Items,
                    opt => opt.MapFrom(up => up.Items.Select(item => new UserPurchaseItemInfo
                    {
                        ItemId = item.ItemId,
                        Number = item.Number,
                        Price = item.Price,
                    })));

            CreateMap<Purchase, MyPurchase>()
                .ForMember(p => p.Users, opt => opt.MapFrom(p => p.UserPurchases))
                .ForMember(p => p.StoreOrder, opt => opt.MapFrom(up => new StoreOrderModel
                    {
                        IsSubmitted = up.StoreOrder.IsSubmitted,
                        StoreId = up.StoreOrder.StoreId
                    })
                );

            CreateMap<UserPurchase, PurchaseWithMe>()
                .ForMember(p => p.Id, opt => opt.MapFrom(up => up.Purchase.Id))
                .ForMember(p => p.Type, opt => opt.MapFrom(up => up.Purchase.Type))
                .ForMember(p => p.CreatedAt, opt => opt.MapFrom(up => up.Purchase.CreatedAt))
                .ForMember(p => p.CreatorName, opt => opt.MapFrom(up => up.Purchase.Creator.UserName))
                .ForMember(p => p.Name, opt => opt.MapFrom(up => up.Purchase.Name))
                .ForMember(p => p.Items, opt => opt.MapFrom(up => up.Items
                    .Select(item => new UserPurchaseItemInfo
                    {
                        ItemId = item.ItemId,
                        Number = item.Number,
                        Price = item.Price
                    }))
                )
                .ForMember(p => p.StoreOrder, opt => opt.MapFrom(up => new StoreOrderModel
                    {
                        IsSubmitted = up.Purchase.StoreOrder.IsSubmitted,
                        StoreId = up.Purchase.StoreOrder.StoreId
                    })
                );
        }
    }

    // TODO map 
    public class AvatarUrlResolver : IValueResolver<UserPurchase, object, string>
    {
        private readonly AvatarsService _avatarsService;

        public AvatarUrlResolver(AvatarsService avatarsService)
        {
            _avatarsService = avatarsService;
        }

        public string Resolve(UserPurchase source, object destination, string destMember, ResolutionContext context)
        {
            return _avatarsService.GetFullAvatarUrl(source.User?.AvatarPath);
        }
    }
}
