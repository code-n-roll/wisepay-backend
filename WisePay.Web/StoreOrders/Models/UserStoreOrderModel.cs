using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class UserStoreOrderModel
    {
        [Required]
        public int UserId { get; set; }
    }
}
