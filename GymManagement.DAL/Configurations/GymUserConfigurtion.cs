using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configurations
{
    public class GymUserConfigurtion<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(p => p.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%_@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone like '010'or Phone like'012' or Phone like'011' or Phone like'015'");
            });

            builder.OwnsOne(x => x.Address,Address=>
            {
                Address.Property(x => x.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
                Address.Property(x => x.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
                Address.Property(x => x.BuildingNumber).HasColumnName("BuildingNumber").HasColumnType("varchar").HasMaxLength(30);


            });

        }
    }
}
