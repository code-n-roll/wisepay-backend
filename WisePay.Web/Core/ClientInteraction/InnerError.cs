using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Core.ClientInteraction
{
    public class InnerError
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
    }
}
