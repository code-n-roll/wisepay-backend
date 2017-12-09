using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WisePay.Web.Teams.Models
{
    public class CreateTeamModel
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<int> UserIds { get; set; }
    }
}
