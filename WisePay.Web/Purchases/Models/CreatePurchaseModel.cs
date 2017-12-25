using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class CreatePurchaseModel
    {
        [Required]
        public string Name { get; set; }
        public decimal? TotalSum { get; set; }
        [Required]
        public IEnumerable<UserPurchaseModel> Users { get; set; }
    }
}
