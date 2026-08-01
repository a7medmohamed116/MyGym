using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.ViewModels.MemberShipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberShipService
    {
        Task<Result<IEnumerable<MemberShipViewModel>>> GetAllMemberShipsAsync(CancellationToken ct = default);
        Task<Result> CreateMemberShipAsync(CreateMembnerShipViewModel model, CancellationToken ct);
        Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMembersForDropDownList(CancellationToken ct =default);
        Task<Result<IEnumerable<PlanSelectListViewModel>>> GetPlansForDropDownList(CancellationToken ct =default);
        Task<Result> DeleteActiveMemberShipp(int memberid , CancellationToken ct =default); // for member who want do cancel  membership  

    }
}
