using System;
using Microsoft.EntityFrameworkCore;
using WisePay.Entities;

namespace WisePay.DataAccess
{
    public class WiseContext : DbContext
    {
        public WiseContext(DbContextOptions<WiseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
