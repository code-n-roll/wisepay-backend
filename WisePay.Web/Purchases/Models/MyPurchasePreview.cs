using System;

namespace WisePay.Web.Purchases.Models
{
    public class MyPurchasePreview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalSum { get; set; }
        public bool IsPayedOff { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
