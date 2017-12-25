using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.ExternalServices.Crawler.Responses
{
    public class StoreResponse
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public ICollection<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
    }
}
