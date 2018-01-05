using System.Collections.Generic;

namespace WisePay.Web.Teams.Models
{
    public class UpdateTeamModel
    {
        public string Name { get; set; }

        public IEnumerable<int> UserIds  { get; set; }
    }
}
