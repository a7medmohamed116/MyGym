using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.BookingViewModels;
using GymManagement.BLL.Services.ViewModels.MemberShipViewModels;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyGym.PL.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await _bookingService.GetAllSessionsAsync(ct);
            if (result.success)
            {
                return View(result.Value);
            }
            TempData["ErrorMessage"] = result.error;
            return View(Enumerable.Empty<SessionViewModel>());

           
        }

        public async Task<IActionResult>GetMembersForOngoingSessions(int id)
        {
            var result = await _bookingService.GetMemberForOngoingSession(id);
            if (result.success)
            {
                return View(result.Value);
            }
            return View(Enumerable.Empty<MemberForSessionViewModel>());
        }
        public async Task<IActionResult> GetMembersForUpcomingSession(int id)
        {
            var result = await _bookingService.GetMemberForUpComingSession(id);
            if (result.success)
            {
                return View(result.Value);
            }
            return View(Enumerable.Empty<MemberForSessionViewModel>());

        }

        public async Task<IActionResult> Create()
        {
            await DropDownList();
            return View();

        }
        [HttpPost]
        public  async Task<IActionResult> Create(CreateBookingViewModel model ,CancellationToken ct =default)
        {
            if (!ModelState.IsValid) {
                await DropDownList();
                return View(model);
            }
            var result = await _bookingService.CreateBooing(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Booking Created Successfully";
                return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
            }
            TempData["ErrorMessage"] = result.error;
            await DropDownList();
            return View(model);

        }

        private async Task DropDownList(CancellationToken ct = default)
        {
            var result = await _bookingService.GetMembersForDropDownList(ct);
            if (result.success)
            {
                ViewBag.Members = new SelectList(result.Value, "Id", "Name");
            }
            else
            {
                ViewBag.Members = new SelectList(Enumerable.Empty<MemberSelectListViewModel>(), "Id", "Name");
            }
        }

        [HttpPost]

        public async Task<IActionResult> Attended(int memberid , int sessionid)
        {
            var result = await _bookingService.MarkAttened(memberid, sessionid);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Member Attended Successfully";
                
            }
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(GetMembersForOngoingSessions) , new {id = sessionid});


        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberid, int sessionid)
        {

            var result = await _bookingService.CancelBooking(memberid, sessionid);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Booking Canceled Successfully";
                
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }
            
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionid });
        }


    }
}//when redirect to action have a paramter to pass must pass it Otherwise it will be zero and load an empty badge
// like  RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionid });
// first we say Index Cause don't have a pass parameter
