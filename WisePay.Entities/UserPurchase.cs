using System;
using System.Collections.Generic;
using System.Text;

namespace WisePay.Entities
{
    public class UserPurchase
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public decimal Sum { get; set; }
        public bool IsPayedOff { get; set; }
    }
}
