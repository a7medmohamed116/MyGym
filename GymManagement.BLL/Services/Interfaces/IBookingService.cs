using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.ViewModels.BookingViewModels;
using GymManagement.BLL.Services.ViewModels.MemberShipViewModels;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForOngoingSession(int sessionid ,CancellationToken ct =default);
        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForUpComingSession(int sessionid ,CancellationToken ct =default);

        Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMembersForDropDownList(CancellationToken ct = default);//pass session id to check if member booked this session before or no
        Task<Result> CreateBooing(CreateBookingViewModel model, CancellationToken ct = default);
        Task<Result> MarkAttened(int memberid ,  int sessionid , CancellationToken ct =default);
        Task<Result> CancelBooking(int memberid, int sessionid, CancellationToken ct = default);


    }
}
