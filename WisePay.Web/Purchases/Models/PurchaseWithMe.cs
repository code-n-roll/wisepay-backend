using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class PurchaseWithMe : PurchaseViewModel
    {
        public PurchaseWithMe()
        {
            IsOwner = false;
        }

        public string CreatorName { get; set; }

        public decimal Sum { get; set; }
        public bool IsPayedOff { get; set; }
    }
}
