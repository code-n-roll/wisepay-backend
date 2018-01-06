using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.ExternalServices.Crawler.Responses
{
    public class StoreResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
    }
}
