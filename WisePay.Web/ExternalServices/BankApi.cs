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
using Newtonsoft.Json.Serialization;
using System.Text;

namespace WisePay.Web.ExternalServices
{
    public class BankApi
    {
        private readonly IConfiguration _config;
        private readonly JsonConfig _jsonConfig;

        public BankApi(IConfiguration configuration, JsonConfig jsonConfig)
        {
            _config = configuration;
            _jsonConfig = jsonConfig;
        }

        public async Task SendMoney(string senderToken, string recipientIdToken, decimal sum)
        {
            try
            {
                await _config["BankAddress"]
                    .AppendPathSegment("sendMoney")
                    .WithHeader("Authorization", senderToken)
                    .PostAsync(new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            userToIdToken = recipientIdToken,
                            amountToSend = sum
                        }, _jsonConfig.Formatter),
                        Encoding.UTF8, "application/json"));
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
                return await _config["BankAddress"]
                    .AppendPathSegment("auth")
                    .PostAsync(new StringContent(
                        JsonConvert.SerializeObject(card, _jsonConfig.Formatter),
                        Encoding.UTF8, "application/json"))
                    .ReceiveJson<BankApiAuthResponse>();
            }
            catch (FlurlHttpException e)
            {
                throw new ApiException((int)e.Call.HttpStatus, e.GetResponseJson<ErrorResponse>().Error);
            }
        }
    }
}
