namespace WisePay.Web.ExternalServices.Crawler.Responses
{
    public class ItemResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
