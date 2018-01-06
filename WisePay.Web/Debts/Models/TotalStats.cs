using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Debts.Models
{
    public class TotalStats
    {
        public decimal MyDebt { get; set; }
        public decimal DebtToMe { get; set; }
    }
}
