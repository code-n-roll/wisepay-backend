using System.Collections.Generic;

namespace WisePay.Web.Core.ClientInteraction
{
    public class ErrorResponse
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<InnerError> InnerErrors { get; set; }
    }
}
