using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WisePay.Web.Internals
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public int Id {
            get
            {
                return int.Parse(_httpContextAccessor.HttpContext
                    .User?.Claims?
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            }
        }

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
