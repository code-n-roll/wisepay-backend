using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Account.Models
{
    public class VerifyResetTokenModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
