using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WisePay.Web.Internals
{
    public class JsonConfig
    {
        private JsonSerializerSettings _formatter = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public JsonSerializerSettings Formatter => _formatter;
    }
}
