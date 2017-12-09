using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class CreatePurchaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public IEnumerable<UserPurchaseModel> Users { get; set; }
    }
}
