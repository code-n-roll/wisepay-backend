using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Purchases.Models
{
    public class SendMoneyModel
    {
        [Required]
        public int UserId { get; set; }

        [Range(0, 1000000, ErrorMessage = "Sum must be greater than zero")]
        [Required]
        public int Sum { get; set; }
    }
}
