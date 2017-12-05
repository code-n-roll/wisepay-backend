using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Users
{
    public class CurrentUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string CardLastFourDigits { get; set; }
    }
}
