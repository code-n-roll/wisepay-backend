using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Account.Models
{
    public class ValidToModel
    {
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
    }
}
