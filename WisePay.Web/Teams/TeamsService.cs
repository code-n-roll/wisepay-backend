using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WisePay.DataAccess;
using WisePay.Entities;

namespace WisePay.Web.Teams
{
    public class TeamsService
    {
        private WiseContext _db;

        public TeamsService(WiseContext db)
        {
            _db = db;
        }

        public async Task<int> CreateTeam(CreateTeamModel model)
        {
            var team = new Team
            {
                Name = model.Name
            };

            _db.Teams.Add(team);
            await _db.SaveChangesAsync();

            _db.UserTeams.AddRange(model.UserIds.Select(id => new UserTeam
            {
                UserId = id,
                TeamId = team.Id
            }));

            await _db.SaveChangesAsync();

            return team.Id;
        }
    }
}
