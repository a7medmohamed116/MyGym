using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.AnalyticsViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnalyticsService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<AnalyticsViewModel>> GetDataAsync(CancellationToken ct = default)
        {
            //No => cause load all data this is not best practice
            //var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();
            //var countmembers = members.Count();

            // i need just number for all entities so i will make count method in the generic repository

            // every request hit database so may be differece in sec so will make var now
            var now = DateTime.Now;
            var AllMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct:ct);
            var ActiveMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(X => X.EndDate> now,ct);
            var AllTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct:ct);
            var UpcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(X => X.StartDate > now, ct);
            var OngoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(X => X.StartDate <= now && X.EndDate >= now);
            var CompletedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(X=> X.EndDate < now);

            var mapped = new AnalyticsViewModel()
            {
                ActiveMembers = ActiveMembers,
                TotalMembers = AllMembers,
                TotalTrainers = AllTrainers,
                UpcomingSessions = UpcomingSessions,
                OngoingSessions = OngoingSessions,
                CompletedSessions = CompletedSessions
            };
            return Result<AnalyticsViewModel>.OK(mapped); 

        }
    }
}
