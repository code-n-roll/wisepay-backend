using System.Collections.Generic;
using WisePay.Web.Users.Models;

namespace WisePay.Web.Teams.Models
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}