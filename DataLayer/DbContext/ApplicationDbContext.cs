using DataLayer.Model.AccountNumber;
using DataLayer.Model.Authenticate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DbContext
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<RegisterUser> RegisterUsers { get; set; }
        public DbSet<AccountNumberGateway> accountNumberGateways { get; set; } 
    }
}