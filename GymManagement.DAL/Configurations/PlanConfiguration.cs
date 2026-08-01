using MyGym.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace MyGym.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable(TB =>
            {
                TB.HasCheckConstraint("DeurationCheck", "DurationDays BETWEEN 1 AND 365");

            });
            
            
            builder.Property(p => p.Name).HasColumnType("varchar")
                                         .HasMaxLength(30);
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.Property(p => p.Price).HasPrecision(10, 2);

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

            //builder.HasData(

            //    new Plan { Id = 1, Name = "Basic Plan", Price = 300m, DurationDays = 30, Description = "Access to gym equipment during staffed hours" },
            //    new Plan { Id = 2, Name = "Standard Plan", Price = 500m, DurationDays = 60, Description = "Includes gym equipment and 2 group classes per week" },
            //    new Plan { Id = 3, Name = "Premium Plan", Price = 900m, DurationDays = 90, Description = "Unlimited access to gym equipment ,classes and sauna" },
            //    new Plan { Id = 4, Name = "Annual Plan", Price = 3000m, DurationDays = 365, Description = "Full year access with personal trainer sessions" }


            //    );
            //add by jsonfile
        }
    }
}
