using WisePay.Entities;

namespace WisePay.Web.Purchases.Models
{
    public class UserPurchaseInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public decimal? Sum { get; set; }
        public PurchaseStatus Status { get; set; }
    }
}
