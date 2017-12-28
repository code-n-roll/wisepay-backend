using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Debts.Models
{
    public class DebtsStats
    {
        public TotalStats TotalStats { get; set; }
        public IEnumerable<UserWithDebt> Users { get; set; }
    }
}
