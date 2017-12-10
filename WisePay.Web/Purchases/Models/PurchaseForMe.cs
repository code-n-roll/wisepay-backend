using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class PurchaseForMe : PurchaseViewModel
    {
        public PurchaseForMe()
        {
            IsMy = false;
        }

        public string CreatorName { get; set; }

        public decimal Amount { get; set; }
        public bool IsPayedOff { get; set; }
    }
}
