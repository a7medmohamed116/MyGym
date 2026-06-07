using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyGym.Context;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepositry : IPlanRepository
    {
        private readonly GymDbContext _context;
        public PlanRepositry(GymDbContext context)
        {
            _context = context;
        } // will kill the new here too and add DI so will Change location of connection to AppSeting.json

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Add(plan);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Remove(plan);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            //if (tracking) return await _context.Plans.ToListAsync(ct);
            //else
            //{
            //    return await _context.Plans.AsNoTracking().ToListAsync(ct);
            //}
            IQueryable<Plan> query = tracking ? _context.Plans : _context.Plans.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Plans.FindAsync(id, ct);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Update(plan);
            return await _context.SaveChangesAsync(ct);
        }
    }
}
