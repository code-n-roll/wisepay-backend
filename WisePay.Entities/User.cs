using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WisePay.Entities
{
    public class User : IdentityUser<int>
    {
        public string BankToken { get; set; }
        public string CardLastFourDigits { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();

        public ICollection<UserPurchase> UserPurchases { get; set; } = new List<UserPurchase>();
    }
}
