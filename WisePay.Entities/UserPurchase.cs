using System.Collections.Generic;

namespace WisePay.Entities
{
    public class UserPurchase
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public decimal? Sum { get; set; }
        public PurchaseStatus Status { get; set; }

        public ICollection<UserPurchaseItem> UserPurchaseItems { get; set; } = new HashSet<UserPurchaseItem>();
    }
}
