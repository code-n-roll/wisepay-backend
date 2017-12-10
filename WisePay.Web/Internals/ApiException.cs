using System;
using System.Collections.Generic;
using WisePay.Web.Core.ClientInteraction;

namespace WisePay.Web.Internals
{
    public class ApiException : Exception
    {
        public ErrorCode Code { get; set; }
        public int HttpStatusCode { get; set; }
        public IEnumerable<InnerError> InnerErrors { get; set; }

        public ApiException(int httpStatusCode, string message, ErrorCode errorCode=ErrorCode.ServerError, IEnumerable<InnerError> innerErrors=null)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            Code = errorCode;
            InnerErrors = innerErrors;
        }
    }
}
