using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // get repository
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity ,new ();
        // save changes
        Task<int> SaveChangesAsync(CancellationToken ct =default); //[Completed]

        //SessionRepo
        public ISessionRepository SessionRepository { get; } // add prop and implement it in unitofwork and give the value from injected value in ctor
        public IMemberShipRepository memberShipRepository { get; }
        public IBookingRepository bookingRepository { get; }


    }
}
