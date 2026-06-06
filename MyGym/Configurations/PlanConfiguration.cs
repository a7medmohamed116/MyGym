using MyGym.Models;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
