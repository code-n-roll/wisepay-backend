using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class SubmitOrderItemModel
    {
        [Required]
        public string ItemId;

        [Required]
        public int Number;

        [Required]
        public decimal Price;
    }
}
