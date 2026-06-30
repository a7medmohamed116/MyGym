using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>>GetAllPlansAsync(CancellationToken ct =default);
        Task<Result<PlanViewModel>> GetPlanDetailsByIdAsync(int planid, CancellationToken ct = default);
        Task<Result> ActivateButtom(int planid ,CancellationToken ct =default);
        Task<Result<UpdatePlanViewModel>>GetPlanToUpdate(int planid, CancellationToken ct =default);
        Task<Result>UpdatePlanAsync(int planid,UpdatePlanViewModel model, CancellationToken ct =default);

    }
}
