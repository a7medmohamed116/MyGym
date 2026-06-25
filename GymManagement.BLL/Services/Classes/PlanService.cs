using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.PlanViewModels;
using GymManagement.DAL.Repositories.Interfaces;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> ActivateButtom(int planid, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planid, ct);
            plan.IsActive = !plan.IsActive;
            _unitOfWork.GetRepository<Plan>().UpdateAsync(plan, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Can NO Deal WIth Plan Now");
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (!plans.Any()) return [];
            var result = _mapper.Map<IEnumerable<PlanViewModel>>(plans);
            return result;

        }

        public async Task<Result<PlanViewModel>> GetPlanDetailsByIdAsync(int planid, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planid, ct);
            if (plan is null) return Result<PlanViewModel>.NotFound("Not Found Plan");
            var result = _mapper.Map<PlanViewModel>(plan);
            return Result<PlanViewModel>.OK(result);
                
           
        }
    }
}
