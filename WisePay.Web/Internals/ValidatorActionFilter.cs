using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WisePay.Web.Core.ClientInteraction;

namespace WisePay.Web.Internals
{
    public class ValidatorActionFilter : IActionFilter
    {
        private JsonConfig _jsonConfig;

        public ValidatorActionFilter(JsonConfig jsonConfig)
        {
            _jsonConfig = jsonConfig;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var result = new ContentResult();
                var errors = new Dictionary<string, string[]>();

                var innerErrors = new List<InnerError>();

                foreach (var valuePair in filterContext.ModelState)
                {
                    foreach (var error in valuePair.Value.Errors)
                    {
                        innerErrors.Add(new InnerError
                        {
                            Code = ErrorCode.ValidationError,
                            Message = error.ErrorMessage
                        });
                    }
                }

                var errorResponse = new ErrorResponse
                {
                    Message = "Validation error",
                    Code = ErrorCode.ValidationError
                };

                if (innerErrors.Count == 1)
                {
                    if (!string.IsNullOrEmpty(innerErrors[0].Message))
                    {
                        errorResponse.Message = innerErrors[0].Message;
                    }
                    else
                    {
                        errorResponse.Message = "Model error";
                    }
                }
                else
                {
                    errorResponse.Code = ErrorCode.MultipleErrors;
                    errorResponse.InnerErrors = innerErrors;
                }

                var content = JsonConvert.SerializeObject(errorResponse, _jsonConfig.Formatter);

                result.Content = content;
                result.ContentType = "application/json";

                filterContext.HttpContext.Response.StatusCode = 422;
                filterContext.Result = result;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
