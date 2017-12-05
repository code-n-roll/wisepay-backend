using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;

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
            var users = _db.Users.Where(u => u.UserName.Contains(query));
            return await users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersInTeam(int teamId)
        {
            var users = await _db.UserTeams
                .Include(ut => ut.User)
                .Where(ut => ut.TeamId == teamId)
                .Select(ut => ut.User)
                .ToListAsync();
            return users;
        }

    }
}
