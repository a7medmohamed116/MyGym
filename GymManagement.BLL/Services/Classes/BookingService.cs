using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.BookingViewModels;
using GymManagement.BLL.Services.ViewModels.MemberShipViewModels;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CancelBooking(int memberid, int sessionid, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.bookingRepository.FirstOrDefaultAsync(X => X.MemberId == memberid && X.SessionId == sessionid);
            if (booking is null) return Result.NotFound("Booking Not Found");
            _unitOfWork.bookingRepository.DeleteAsync(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Booking");
        }

        public async Task<Result> CreateBooing(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var sessionexist = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId, ct); 
            if (sessionexist is null) return Result.Fail("Session Not Exist");

            if (sessionexist.StartDate <= DateTime.Now) return Result.Fail("Can't Book Session Already Started");
            var memberexist = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.MemberId);
            if (memberexist is null) return Result.Fail("Member Not Exist");
            var membership = await _unitOfWork.memberShipRepository.AnyAsync(X=>X.MemberId ==  model.MemberId && X.EndDate > DateTime.Now ,ct);
            if (!membership) return Result.Fail("Member Must Have Active MemberShip First");
            var memberwithsamesessionberbefore = await _unitOfWork.bookingRepository.AnyAsync(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId);
            if (memberwithsamesessionberbefore)
            {
                return Result.Fail("Member Already Booked This Session Before");

            }
            var bookedslots = await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(model.SessionId);
            if (bookedslots >= sessionexist.Capacity) return Result.Fail("No Available Slots , Session Full Capacity");

              var mapped = _mapper.Map<Booking>(model);
            _unitOfWork.bookingRepository.AddAsync(mapped);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed TO Create Booking");



        }

        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetSessionsWithTrainerAndCategory(X => X.EndDate >= DateTime.UtcNow, ct); // fillter ongoing and upcoming only 
            if (!sessions.Any()) return Result<IEnumerable<SessionViewModel>>.NotFound("No Sessions Available");
            var mapped =  _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach(var session in mapped)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(session.Id, ct);
            }
            return Result<IEnumerable<SessionViewModel>>.OK(mapped);
        }

        public async Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForOngoingSession(int sessionid, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.bookingRepository.GetBySessionId(sessionid,ct);
            var mapped = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
            return Result<IEnumerable<MemberForSessionViewModel>>.OK(mapped);
        }

        public async Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForUpComingSession(int sessionid, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.bookingRepository.GetBySessionId(sessionid, ct);
            var mapped = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
            return Result<IEnumerable<MemberForSessionViewModel>>.OK(mapped);
        }

        public async Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMembersForDropDownList( CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
            return Result<IEnumerable<MemberSelectListViewModel>>.OK(mapped);
        }

        public async Task<Result> MarkAttened(int memberid, int sessionid, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.bookingRepository.FirstOrDefaultAsync(X => X.MemberId == memberid && X.SessionId == sessionid);
            if (booking is null) return Result.NotFound("Booking Not Found");
            booking.IsAttened = true;
            booking.UpdatedAt = DateTime.Now;
            _unitOfWork.bookingRepository.UpdateAsync(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Mark As Attend"); 
        }
    }
}
