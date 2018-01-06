using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;
using WisePay.Web.Teams.Models;

namespace WisePay.Web.Teams
{
    public class TeamsService
    {
        private WiseContext _db;

        public TeamsService(WiseContext db)
        {
            _db = db;
        }

        public async Task<Team> CreateTeam(CreateTeamModel model, int adminId)
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

            var userTeams = await _db.UserTeams
                .Include(ut => ut.User)
                .Where(ut => ut.TeamId == team.Id)
                .ToListAsync();

            team.UserTeams = userTeams;

            return team;
        }

        public async Task<IEnumerable<Team>> GetUserTeams(int userId)
        {
            return await _db.Teams
                .Include(t => t.UserTeams)
                .ThenInclude(ut => ut.User)
                .Where(t => t.UserTeams
                    .Where(ut => ut.UserId == userId)
                    .Count() != 0)
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

            await UpdateTeamUsers(teamId, model.UserIds, currentUserId);

            await _db.SaveChangesAsync();
        }

        private async Task UpdateTeamUsers(int teamId, IEnumerable<int> userIds, int currentUserId)
        {
            if (userIds == null)
                return;

            var team = await _db.Teams
                .Include(t => t.UserTeams)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            if (team == null) throw new ApiException(404, "Team not found", ErrorCode.NotFound);

            if (team.AdminId != currentUserId)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            if (!userIds.Contains(team.AdminId))
                throw new ApiException(400, "Can't delete admin from team", ErrorCode.InvalidRequestFormat);

            _db.UserTeams.RemoveRange(team.UserTeams);
            await _db.SaveChangesAsync();

            _db.UserTeams.AddRange(userIds.Select(id => new UserTeam
            {
                UserId = id,
                TeamId = team.Id
            }));

            await _db.SaveChangesAsync();
        }

        public async Task DeleteTeam(int teamId, int currentUserId)
        {
            var team = await _db.Teams.FindAsync(teamId);
            if (team == null)
                throw new ApiException(400, "Team not found", ErrorCode.NotFound);

            if (team.AdminId != currentUserId)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
        }

        public async Task LeaveTeam(int teamId, int currentUserId)
        {
            var team = await _db.Teams
                .Include(t => t.UserTeams)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            if (team == null)
                throw new ApiException(400, "Team not found", ErrorCode.NotFound);

            if (currentUserId == team.AdminId)
                throw new ApiException(400, "Can't leave team where you are an admin",
                    ErrorCode.AuthError);

            var userTeam = team.UserTeams
                .Where(ut => ut.UserId == currentUserId)
                .FirstOrDefault();

            if (userTeam == null)
                throw new ApiException(400, "Not a member of the team", ErrorCode.AuthError);

            _db.UserTeams.Remove(userTeam);
            await _db.SaveChangesAsync();
        }
    }
}
