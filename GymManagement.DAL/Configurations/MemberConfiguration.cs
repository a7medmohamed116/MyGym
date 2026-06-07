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
    public class MemberConfiguration : GymUserConfigurtion<Member> ,IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(p => p.CreatedAt).HasColumnName("JoinDate")
                                              .HasDefaultValueSql("GETDATE()");
            base.Configure(builder); // with new and base will implement the two congig
        }
    }
}
