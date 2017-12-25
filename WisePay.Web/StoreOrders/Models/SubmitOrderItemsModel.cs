using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class SubmitOrderItemsModel
    {
        [Required]
        public IEnumerable<SubmitOrderItemModel> Items { get; set; }
    }
}
