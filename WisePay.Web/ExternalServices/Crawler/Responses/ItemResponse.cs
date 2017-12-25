using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.ExternalServices.Crawler.Responses
{
    public class ItemResponse
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string Description { get; set; }
    }
}
