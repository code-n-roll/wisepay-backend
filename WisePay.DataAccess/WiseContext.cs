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

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Team> Teams { get; set; }

        // Connection tables
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserPurchase> UserPurchases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserTeam>()
                .ToTable("UserTeams")
                .HasKey(t => new { t.UserId, t.TeamId });

            builder.Entity<UserPurchase>()
                .ToTable("UserPurchases")
                .HasKey(t => new { t.UserId, t.PurchaseId });
        }
    }
}
