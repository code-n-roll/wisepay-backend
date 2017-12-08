using System.Collections.Generic;
using WisePay.Web.Users;

namespace WisePay.Web.Teams
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}