using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WisePay.Web.Teams
{
    public class CreateTeamModel
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<int> UserIds { get; set; }
    }
}
