using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;

namespace WisePay.Web.Users
{
    public class UsersService
    {
        private WiseContext _db;

        public UsersService(WiseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllByQuery(string query)
        {
            IQueryable<User> users = null;

            if (string.IsNullOrWhiteSpace(query)) {
                users = _db.Users;
            }
            else {
                users = _db.Users.Where(u => u.UserName.Contains(query));
            }

            return await users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersInTeam(int teamId)
        {
            var team = await _db.Teams
                .Include(t => t.UserTeams)
                .ThenInclude(ut => ut.User)
                .Where(t => t.Id == teamId)
                .FirstOrDefaultAsync();

            if (team == null) throw new ApiException(404, "Team with this id is not found", ErrorCode.NotFound);

            return team.UserTeams.Select(ut => ut.User);
        }

    }
}
