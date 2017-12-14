using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using WisePay.Web.Account.Models;
using WisePay.Web.ExternalServices.Responses;
using WisePay.Web.Internals;

namespace WisePay.Web.ExternalServices
{
    public class BankApi
    {
        private readonly IConfiguration _config;

        public BankApi(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendMoney(string senderToken, string recipientIdToken, decimal sum)
        {
            try
            {
                await _config["BankToken"]
                    .AppendPathSegment("sendMoney")
                    .WithHeader("Authorization", senderToken)
                    .PostJsonAsync(new
                    {
                        userToIdToken = recipientIdToken,
                        amountToSend = sum
                    });
            }
            catch (FlurlHttpException e)
            {
                if (e.Call.HttpStatus == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ApiException(400, "Not enough money");
                } 
            }
        }

        public async Task<BankApiAuthResponse> Authenticate(BankCardModel card)
        {
            try
            {
                return await _config["BankToken"]
                    .AppendPathSegment("auth")
                    .PostJsonAsync(card)
                    .ReceiveJson<BankApiAuthResponse>();
            }
            catch (FlurlHttpException e)
            {
                throw new ApiException((int)e.Call.HttpStatus, e.GetResponseJson<ErrorResponse>().Error);
            }
        }
    }
}
