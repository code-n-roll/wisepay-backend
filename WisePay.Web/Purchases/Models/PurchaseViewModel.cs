using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsOwner { get; protected set; }

        public StoreOrderModel StoreOrder { get; set; }
    }
}
