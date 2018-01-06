using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Account.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }
}
