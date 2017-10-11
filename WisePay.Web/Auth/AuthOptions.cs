using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WisePay.Web.Auth
{
    public class AuthOptions
    {
        public const string Issuer = "WisePay.Web.Token";
        public const string Audience = "WisePay.Client";
        public const int Lifetime = 30; // in days

        public static SymmetricSecurityKey SymmetricSecurityKey =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));

        private const string Key = "mysupersecret_secretkey!123";
    }
}
