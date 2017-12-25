using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using WisePay.Web.Internals;
using WisePay.Web.ExternalServices.Bank.Responses;
using WisePay.Web.ExternalServices.Crawler.Responses;

namespace WisePay.Web.ExternalServices.Crawler
{
    public class CrawlerApi
    {
        private readonly IConfiguration _config;
        private readonly JsonConfig _jsonConfig;

        public CrawlerApi(IConfiguration configuration, JsonConfig jsonConfig)
        {
            _config = configuration;
            _jsonConfig = jsonConfig;
        }

        public async Task<List<StoreResponse>> GetStores()
        {
            try
            {
                return await _config["CrawlerAddress"]
                    .AppendPathSegment("stores")
                    .GetAsync()
                    .ReceiveJson<List<StoreResponse>>();
            }
            catch (FlurlHttpException e)
            {
                throw new ApiException((int)e.Call.HttpStatus, e.GetResponseJson<ErrorResponse>().Error);
            }
        }

        public async Task<List<CategoryResponse>> GetCategories(string storeId)
        {
            try
            {
                return await _config["CrawlerAddress"]
                    .AppendPathSegment("categories")
                    .SetQueryParam("storeId", storeId)
                    .GetAsync()
                    .ReceiveJson<List<CategoryResponse>>();
            }
            catch (FlurlHttpException e)
            {
                throw new ApiException((int)e.Call.HttpStatus, e.GetResponseJson<ErrorResponse>().Error);
            }
        }

        public async Task<List<ItemResponse>> GetItems(string categoryId, IEnumerable<string> itemIds)
        {
            try
            {
                return await _config["CrawlerAddress"]
                    .AppendPathSegment("items")
                    .SetQueryParam("categoryId", categoryId)
                    .SetQueryParam("ids", string.Join(",", itemIds))
                    .GetAsync()
                    .ReceiveJson<List<ItemResponse>>();
            }
            catch (FlurlHttpException e)
            {
                throw new ApiException((int)e.Call.HttpStatus, e.GetResponseJson<ErrorResponse>().Error);
            }
        }
    }
}
