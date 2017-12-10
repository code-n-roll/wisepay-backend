using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class MyPurchase : PurchaseViewModel
    {
        public MyPurchase()
        {
            IsMy = true;
        }

        public decimal TotalAmount { get; set; }
        public bool IsPayedOff { get; set; }

        public IEnumerable<UserPurchaseInfo> Users { get; set; }
    }
}
