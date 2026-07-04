using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyGym.Configurations;
using MyGym.Models;
using System.Reflection;

namespace MyGym.Context
{
    public class GymDbContext : IdentityDbContext<ApplicationUser> ////
    {
        // do need onconfiguring if we are using dependency injection to pass options
        public GymDbContext(DbContextOptions<GymDbContext> options): base(options) { }
        //base have access to reach appsettings.json to get connection string
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // apply configuration of identity
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        //public DbSet<ApplicationUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
        ////Relation M:M users and roles
        //public DbSet<IdentityUserRole<string>> UserRoles { get; set; }

    }

}
