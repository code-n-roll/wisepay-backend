using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Core.ClientInteraction
{
    public enum ErrorCode
    {
        ServerError,
        MultipleErrors,

        AuthError,
        InvalidCredentials,
        ValidationError,

        NotFound,
        InvalidRequestFormat
    }
}
