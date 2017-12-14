using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.ExternalServices.Responses
{
    public class BankApiAuthResponse
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
    }
}
