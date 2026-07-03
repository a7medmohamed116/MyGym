using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        //Number of members booked for a specific session
        Task<List<Booking>> GetBySessionId(int sessionid ,CancellationToken ct =default);
    }
}
