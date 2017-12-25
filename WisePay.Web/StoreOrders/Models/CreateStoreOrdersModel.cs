using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Purchases.Models
{
    public class CreateStoreOrdersModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string StoreId { get; set; }

        [Required]
        public IEnumerable<UserStoreOrderModel> Users { get; set; }
    }
}
