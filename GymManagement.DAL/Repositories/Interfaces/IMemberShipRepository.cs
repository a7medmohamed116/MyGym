using GymManagement.DAL.Models;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<Membership>
    {
        Task<IEnumerable<Membership>> GetMembershipsWithPlanAndMember(Expression<Func<Membership,bool>>? filter = null  , CancellationToken ct =default); //filteration on memmberships by funcy


    }
}
