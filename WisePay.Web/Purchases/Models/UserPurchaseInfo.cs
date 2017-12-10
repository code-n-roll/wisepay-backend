namespace WisePay.Web.Purchases.Models
{
    public class UserPurchaseInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public decimal Amount { get; set; }
        public bool IsPayedOff { get; set; }
    }
}
