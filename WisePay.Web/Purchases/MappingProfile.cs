using System.Linq;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Purchases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Purchase, MyPurchase>()
                .ForMember(p => p.Users,
                    opt => opt.MapFrom(t => t.UserPurchases.Select(up => new UserPurchaseInfo
                    {
                        Sum = up.Sum,
                        Status = up.Status,
                        UserId = up.UserId,
                        Username = up.User != null ? up.User.UserName : null,
                        Items = up.Items.Select(item => new UserPurchaseItemInfo
                        {
                            ItemId = item.ItemId,
                            Number = item.Number,
                            Price = item.Price
                        })
                    }))
                )
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
}
