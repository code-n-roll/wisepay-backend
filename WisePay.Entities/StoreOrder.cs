namespace WisePay.Entities
{
    public class StoreOrder
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public string StoreId  { get; set; }
        public bool IsSubmitted  { get; set; }
    }
}
