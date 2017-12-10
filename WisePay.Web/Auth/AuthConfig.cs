using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WisePay.Web.Auth
{
    public class AuthConfig
    {
        public const string Issuer = "WisePay.Web.Token";
        public const string Audience = "WisePay.Client";
        public const int Lifetime = 30; // in days

        public static SymmetricSecurityKey SymmetricSecurityKey =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));

        private const string Key = "mysupersecret_secretkey!123";
    }
}
