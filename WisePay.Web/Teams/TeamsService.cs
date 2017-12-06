using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Team>> GetUserTeams(int userId)
        {
            return await _db.UserTeams
                .Include(ut => ut.User)
                .Where(ut => ut.UserId == userId)
                .Select(ut => ut.Team)
                .ToListAsync();
        }

        public async Task<Team> GetTeam(int teamId) {
            return await _db.Teams.FindAsync(teamId);
        }
    }
}
