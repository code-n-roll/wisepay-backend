using System;
using System.Collections.Generic;
using System.Text;

namespace WisePay.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public decimal TotalSum { get; set; }
        public bool IsPayedOff { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<UserPurchase> UserPurchases { get; set; } = new List<UserPurchase>();
    }
}
