using Microsoft.EntityFrameworkCore;
using MyGym.Configurations;
using MyGym.Models;

namespace MyGym.Context
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //should be in appseting.json
            optionsBuilder.UseSqlServer("Server=AHMED_MOHAMED;Database=MyGym;Trusted_Connection=True;TrustServerCertificate=true");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }
    }

}
