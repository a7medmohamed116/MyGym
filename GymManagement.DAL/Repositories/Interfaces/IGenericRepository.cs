using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        Task<int> AddAsync(TEntity entity, CancellationToken ct = default);
        Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default);
        Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking =false ,CancellationToken ct =default);

    }
}
