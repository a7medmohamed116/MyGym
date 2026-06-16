using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyGym.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {

        //Connection
        private readonly GymDbContext _dbContext;
        public GenericRepository(GymDbContext context)
        {
            _dbContext = context;
            // must regeister service in program.cs 
        }


        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            
            //if (tracking) return await _context.Plans.ToListAsync(ct);
            //else
            //{
            //    return await _context.Plans.AsNoTracking().ToListAsync(ct);
            //}



            IQueryable<TEntity> query = tracking ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id, ct); 
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
