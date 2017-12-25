using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.ExternalServices.Crawler.Responses
{
    public class CategoryResponse
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<ItemResponse> Items { get; set; } = new List<ItemResponse>();
    }
}
