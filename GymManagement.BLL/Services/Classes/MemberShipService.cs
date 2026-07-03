using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.MemberShipViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipService(IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public  async Task<Result<IEnumerable<MemberShipViewModel>>> GetAllMemberShipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.memberShipRepository.GetMembershipsWithPlanAndMember(x => x.EndDate > DateTime.Now, ct); // isactive can't work on iquerable cause it computed prop
            //if (!memberships.Any()) return Result<IEnumerable<MemberShipViewModel>>.NotFound("No Available Active MemberShips");
            var mapped =  _mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
            return Result<IEnumerable<MemberShipViewModel>>.OK(mapped);

        }


        public async Task<Result> CreateMemberShipAsync(CreateMembnerShipViewModel model, CancellationToken ct)
        {
            var memberexist = await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Id == model.MemberId , ct);
            if (!memberexist ) return Result.NotFound("Member Must Be Exist");
            var planexist = await _unitOfWork.GetRepository<Plan>().AnyAsync(X => X.Id == model.PlanId);
            if (!planexist) return Result.NotFound("Plan Must Be Exist");
            var hasactivemembership = await _unitOfWork.memberShipRepository.AnyAsync(X => X.MemberId == model.MemberId && X.EndDate > DateTime.Now ,ct); // X.isactive?
            if (hasactivemembership) return Result.Fail("Member Already Have One Active Membership");
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct);
            if (!plan.IsActive) return Result.Fail("Plan Is Not Active Right Now"); 
            var membership =  _mapper.Map<Membership>(model);
            membership.EndDate =    (model.StartDate??DateTime.Now).AddDays(plan.DurationDays);
            _unitOfWork.memberShipRepository.AddAsync(membership);

            var result =  await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Create MemberShip");
            


        }

        public async Task<Result> DeleteActiveMemberShipp(int memberid, CancellationToken ct = default)
        {
            var activemembership = await _unitOfWork.memberShipRepository.FirstOrDefaultAsync(X => X.MemberId == memberid && X.EndDate > DateTime.Now,true);
            if(activemembership == null) return Result.NotFound("No Active Membership Found For This Member");

            _unitOfWork.memberShipRepository.DeleteAsync(activemembership);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return  result >0 ?  Result.OK() : Result.Fail("Failed To Delete MemberShip");

        }


        public async Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMembersForDropDownList(CancellationToken ct = default)
        {
            var members =await  _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
             var mapped =  _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
            return Result<IEnumerable<MemberSelectListViewModel>>.OK(mapped);
        }

        public async Task<Result<IEnumerable<PlanSelectListViewModel>>> GetPlansForDropDownList(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            var mapped = _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
            return Result<IEnumerable<PlanSelectListViewModel>>.OK(mapped);
        }
    }
}
