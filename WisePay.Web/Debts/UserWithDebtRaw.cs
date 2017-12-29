using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WisePay.Entities;

namespace WisePay.Web.Debts.Models
{
    public class UserWithDebtRaw
    {
        public User User { get; set; }
        public decimal Debt { get; set; }
    }
}
