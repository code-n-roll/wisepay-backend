using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Core.Emails
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<string>();
            FromAddresses = new List<string>();
        }

        public List<string> ToAddresses { get; set; }
        public List<string> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
