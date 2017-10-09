using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WisePay.Entities;

namespace WisePay.DataAccess
{
    public class WiseContext : IdentityDbContext<User>
    {
        public WiseContext(DbContextOptions<WiseContext> options) : base(options) { }
    }
}
