using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{

    // make it base entity to tell help just work with tables on data base only 
    // have problem with gymuser so we will make it abstract class and tell the interface with new() to kill the problem . with new will only work with classes that have a parameterless constructor.
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        // add update delete will return int cause # of row effected

        //
        void AddAsync(TEntity entity, CancellationToken ct = default);
        void UpdateAsync(TEntity entity, CancellationToken ct = default);
        void DeleteAsync(TEntity entity, CancellationToken ct = default);
        // will refactor from "Task<int>" to void cause we will use unit of work pattern to save changes in one place and not in every repository 
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking =false ,CancellationToken ct =default);

        //check method can check with it in every place
        Task<bool>AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
        //x => x.Email

        //check user has active membership not 
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = false, CancellationToken ct = default);

        Task<int> CountAsync(Expression<Func<TEntity ,bool>>? expression = null ,CancellationToken ct = default);
    }
}
