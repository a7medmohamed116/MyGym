using Microsoft.EntityFrameworkCore;
using MyGym.Configurations;
using MyGym.Models;

namespace MyGym.Context
{
    public class GymDbContext : DbContext
    {
        // do need onconfiguring if we are using dependency injection to pass options
        public GymDbContext(DbContextOptions<GymDbContext> options): base(options) { }
        //base have access to reach appsettings.json to get connection string
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }
    }

}
