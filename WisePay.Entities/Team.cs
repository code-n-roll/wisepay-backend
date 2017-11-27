using System;
using System.Collections.Generic;
using System.Text;

namespace WisePay.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
    }
}
