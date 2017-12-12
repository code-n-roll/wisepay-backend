using System;
using System.Collections.Generic;
using System.Text;

namespace WisePay.Entities
{
    public class PaymentHistoryItem
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }
        public decimal Sum { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
}
