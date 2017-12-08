using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;

namespace WisePay.Web.Teams
{
    public class TeamsService
    {
        private WiseContext _db;

        public TeamsService(WiseContext db)
        {
            _db = db;
        }

        public async Task<int> CreateTeam(CreateTeamModel model, int adminId)
        {
            var team = new Team
            {
                Name = model.Name,
                AdminId = adminId
            };

            _db.Teams.Add(team);
            await _db.SaveChangesAsync();

            _db.UserTeams.AddRange(model.UserIds.Select(id => new UserTeam
            {
                UserId = id,
                TeamId = team.Id
            }));

            _db.UserTeams.Add(new UserTeam
            {
                UserId = adminId,
                TeamId = team.Id
            });

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
            return await _db.Teams
                .Include(t => t.UserTeams)
                .ThenInclude(ut => ut.User)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateTeam(int teamId, UpdateTeamModel model, int currentUserId)
        {
            var team = await _db.Teams.FindAsync(teamId);

            if (team == null) throw new ApiException(404, "Team not found", ErrorCode.NotFound);

            if (team.AdminId != currentUserId)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            team.Name = string.IsNullOrWhiteSpace(model.Name) ? team.Name : model.Name;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateTeamUsers(int teamId, UpdateTeamUsersModel model, int currentUserId)
        {
            var team = await _db.Teams
                .Include(t => t.UserTeams)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            if (team == null) throw new ApiException(404, "Team not found", ErrorCode.NotFound);

            if (team.AdminId != currentUserId)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            if (!model.UserIds.Contains(team.AdminId))
                throw new ApiException(400, "Can't delete admin from team", ErrorCode.InvalidRequestFormat);

            _db.UserTeams.RemoveRange(team.UserTeams);
            await _db.SaveChangesAsync();

            _db.UserTeams.AddRange(model.UserIds.Select(id => new UserTeam
            {
                UserId = id,
                TeamId = team.Id
            }));

            await _db.SaveChangesAsync();
        }
    }
}
