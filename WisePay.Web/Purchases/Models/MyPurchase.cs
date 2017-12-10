using System;
using System.Collections.Generic;

namespace WisePay.Web.Purchases.Models
{
    public class MyPurchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPayedOff { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<UserPurchaseInfo> Users { get; set; }
    }
}
