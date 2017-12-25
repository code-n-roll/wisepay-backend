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
                        Username = up.User != null ? up.User.UserName : null
                    }))
                );

            CreateMap<UserPurchase, PurchaseWithMe>()
                .ForMember(p => p.Id, opt => opt.MapFrom(up => up.Purchase.Id))
                .ForMember(p => p.CreatedAt, opt => opt.MapFrom(up => up.Purchase.CreatedAt))
                .ForMember(p => p.CreatorName, opt => opt.MapFrom(up => up.Purchase.Creator.UserName))
                .ForMember(p => p.Name, opt => opt.MapFrom(up => up.Purchase.Name))
                .ForMember(p => p.Items, opt => opt.MapFrom(up => up.Items));
        }
    }
}
