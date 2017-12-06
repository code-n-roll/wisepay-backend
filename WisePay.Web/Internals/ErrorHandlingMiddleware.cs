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
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
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

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
