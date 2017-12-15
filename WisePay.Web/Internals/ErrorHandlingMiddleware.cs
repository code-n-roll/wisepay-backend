using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WisePay.Web.Core.ClientInteraction;

namespace WisePay.Web.Internals
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonConfig _jsonConfig;

        public ErrorHandlingMiddleware(RequestDelegate next, JsonConfig jsonConfig)
        {
            _next = next;
            _jsonConfig = jsonConfig;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorResponse response = null;
            context.Response.ContentType = "application/json";

            if (exception is ApiException e)
            {
                context.Response.StatusCode = e.HttpStatusCode;
                response = new ErrorResponse
                {
                    Code = e.Code,
                    Message = e.Message,
                    InnerErrors = null
                };
            }
            else
            {
                context.Response.StatusCode = 500;
                if (!string.IsNullOrWhiteSpace(exception.Message))
                {
                    response = new ErrorResponse
                    {
                        Code = ErrorCode.ServerError,
                        Message = exception.Message
                    };
                }
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _jsonConfig.Formatter));
        }
    }
}
