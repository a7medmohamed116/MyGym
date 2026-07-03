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
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(p => p.CreatedAt).HasColumnName("StartDate")
                                             .HasDefaultValueSql("GETDATE()");
            builder.HasKey(b => b.Id); 

            builder.HasOne(m => m.Plan)
                   .WithMany(p => p.Memberships)
                   .HasForeignKey(m => m.PlanId) 
                   .OnDelete(DeleteBehavior.Restrict); // must not delete a plan if there are memberships associated with it  mustt make manual 
            
            builder.HasOne(m => m.Member)
                   .WithMany(p => p.Memberships)
                   .HasForeignKey(m => m.MemberId)
                   .OnDelete(DeleteBehavior.Cascade); //by default, when a member is deleted, their memberships will also be deleted


        }
    }
}
