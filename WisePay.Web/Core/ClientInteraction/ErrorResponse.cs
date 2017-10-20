using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Core.ClientInteraction
{
    public class ErrorResponse
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<ErrorResponse> InnerErrors { get; set; }
    }
}
