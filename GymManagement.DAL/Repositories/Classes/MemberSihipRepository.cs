using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyGym.Context;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MemberSihipRepository : GenericRepository<Membership>, IMemberShipRepository
    {
        private readonly GymDbContext _context;

        public MemberSihipRepository(GymDbContext dbcontext) : base(dbcontext)
        {
            _context = dbcontext;
        }

  
        public async Task<IEnumerable<Membership>> GetMembershipsWithPlanAndMember(Expression<Func<Membership, bool>>? filter = null , CancellationToken ct =default)
        {
            IQueryable<Membership> query = _context.Memberships.AsNoTracking().Include(X => X.Plan).Include(X => X.Member);
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync(ct);
        }

        
    }
}
