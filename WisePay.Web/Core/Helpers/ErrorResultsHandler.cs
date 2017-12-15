using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;

namespace WisePay.Web.Core.Helpers
{
    public static class ErrorResultsHandler
    {
        public static void ThrowIfIdentityError(IdentityResult result)
        {
            if (result == null)
                throw new ApiException(400, "Something strange happened", ErrorCode.ServerError);

            if (result.Succeeded) return;

            if (result.Errors == null)
                throw new ApiException(400, "Something strange happened", ErrorCode.ServerError);

            if (result.Errors.Count() == 1)
                throw new ApiException(400, result.Errors.ElementAt(0).Description, ErrorCode.AuthError);

            var innerErrors = new List<InnerError>();

            foreach (var error in result.Errors)
            {
                innerErrors.Add(new InnerError
                {
                    Code = ErrorCode.AuthError,
                    Message = error.Description
                });
            }

            throw new ApiException(400, "", ErrorCode.MultipleErrors, innerErrors);
        }
    }
}
