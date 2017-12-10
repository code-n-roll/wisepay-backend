using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class UserPurchaseModel
    {
        [Required]
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
