namespace WisePay.Entities
{
    public class UserPurchaseItem
    {
        public int Id { get; set; }

        public long ItemId { get; set; }
        public long Number { get; set; }
        public decimal Sum { get; set; }
    }
}
