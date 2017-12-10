using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class UserPurchaseInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public decimal Sum { get; set; }
        public bool IsPayedOff { get; set; }
    }
}
