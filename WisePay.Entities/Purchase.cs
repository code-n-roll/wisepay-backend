using System;
using System.Collections.Generic;

namespace WisePay.Entities
{

    public class Purchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PurchaseType Type { get; set; }
        public decimal? TotalSum { get; set; }
        public bool IsPayedOff { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public StoreOrder StoreOrder { get; set; }

        public ICollection<UserPurchase> UserPurchases { get; set; } = new List<UserPurchase>();
    }
}
