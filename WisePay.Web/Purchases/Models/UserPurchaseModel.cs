using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class UserPurchaseModel
    {
        [Required]
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
