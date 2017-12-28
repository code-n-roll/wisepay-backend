using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Account.Models
{
    public class ConfirmResetPasswordModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string PasswordConfirmation { get; set; }
    }
}
