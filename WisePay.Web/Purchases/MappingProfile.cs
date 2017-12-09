﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WisePay.Entities;
using WisePay.Web.Purchases.Models;
using WisePay.Web.Teams.Models;

namespace WisePay.Web.Purchases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Purchase, MyPurchasePreview>();
            CreateMap<Purchase, MyPurchase>()
                .ForMember(p => p.Users,
                    opt => opt.MapFrom(t => t.UserPurchases.Select(up => new UserPurchaseInfo
                    {
                        Amount = up.Sum,
                        IsPayedOff = up.IsPayedOff,
                        UserId = up.UserId,
                        Username = up.User != null ? up.User.UserName : null
                    }))
                );

            CreateMap<UserPurchase, PurchaseForMe>()
                .ForMember(p => p.CreatedAt, opt => opt.MapFrom(up => up.Purchase.CreatedAt))
                .ForMember(p => p.CreatorName, opt => opt.MapFrom(up => up.Purchase.Creator.UserName))
                .ForMember(p => p.Name, opt => opt.MapFrom(up => up.Purchase.Name));
        }
    }
}