namespace WisePay.Entities
{
    public class UserPurchaseItem
    {
        public int Id { get; set; }

        public UserPurchase UserPurchase { get; set; }

        public string ItemId { get; set; }
        public long Number { get; set; }
        public decimal Price { get; set; }
    }
}
