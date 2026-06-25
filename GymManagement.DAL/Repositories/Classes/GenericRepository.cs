using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyGym.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


        public async void AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Add(entity); // add local
            //return await _dbContext.SaveChangesAsync(ct); // no need to save changes here because we will save changes in unit of work class  im (add ,update,delete)
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
        {
             return await _dbContext.Set<TEntity>().AnyAsync(expression, ct); // go mathch if anything match this expression in the database or not
        }

        public async void DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = false, CancellationToken ct = default)
        {
            var query = tracking ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().AsNoTracking();
            return await query.FirstOrDefaultAsync(expression, ct);
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

        public async void UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Update(entity);
            
        }
    }
}
