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
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext) :base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<List<Booking>> GetBySessionId(int sessionid, CancellationToken ct = default)
        {
            return _dbContext.Bookings.AsNoTracking().Include(X => X.Member)
                                                     .Where(X => X.SessionId == sessionid)
                                                     .ToListAsync(ct);
        }
    }
}
