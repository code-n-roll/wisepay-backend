namespace WisePay.Entities
{
    public class StoreOrder
    {
        public int Id { get; set; }

        public long StoreId  { get; set; }
        public bool IsSubmitted  { get; set; }
    }
}
