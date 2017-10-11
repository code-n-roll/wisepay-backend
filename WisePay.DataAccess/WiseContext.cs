using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WisePay.Entities;
using Microsoft.AspNetCore.Identity;

namespace WisePay.DataAccess
{
    public class WiseContext : IdentityDbContext<User, Role, int>
    {
        public WiseContext(DbContextOptions<WiseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
