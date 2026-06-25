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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {

        //databse connection
        private readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) :base(dbContext) // exist inheritance must pass the base
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountOfBookedSlotsAsync(int sessionid, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.AsNoTracking().CountAsync(X => X.SessionId == sessionid);
        }

        public async Task<IEnumerable<Session>> GetSessionsWithTrainerAndCategory(CancellationToken ct = default)
        {
            //sessions.include trainer , category 
            var query =  _dbContext.Sessions.AsNoTracking().Include(X => X.Trainer).Include(X => X.Category);
            return await query.ToListAsync();
        }

        public async Task<Session?> GetSessionWithTrainerAndCategory(int sessionid, CancellationToken ct = default)
        {
            return await  _dbContext.Sessions.AsNoTracking().Include(X => X.Trainer).Include(X => X.Category).FirstOrDefaultAsync(X => X.Id == sessionid);
           
        }
    }
}
