using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Internals
{
    public interface ICurrentUserAccessor
    {
        int Id { get; }
    }
}
