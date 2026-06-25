using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        
        Task<IEnumerable<Session>> GetSessionsWithTrainerAndCategory(CancellationToken ct =default); // session not session view model cause here deal with database

        Task<int> CountOfBookedSlotsAsync(int sessionid , CancellationToken ct =default);

        // Session with load trainer and category for sessiondetails
        Task<Session?>GetSessionWithTrainerAndCategory(int sessionid , CancellationToken ct =default);
       




    }
}
