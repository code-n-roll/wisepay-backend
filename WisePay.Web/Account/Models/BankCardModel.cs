using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Account.Models
{
    public class BankCardModel
    {
        [Required, StringLength(16, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        public Holder Holder { get; set; }
        [Required]
        public int Cvc { get; set; }
        [Required]
        public ValidToModel ValidTo { get; set; }
    }
}
