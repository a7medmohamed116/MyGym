using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MyGym.PL.Controllers
{
    public class MemberController : Controller
    {


        //MemberService
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }


        #region Get members
        // Get :: baseurl/member/index => all members

        public async Task<IActionResult> Index( CancellationToken ct = default)
        {

            var members = await _memberService.GetAllAsync(ct);
            return View(members);
        }



        // Get :: baseurl/member/details/{id} => member details  specific member
        // get :: baseurl/member/HealthsRecordDetalis/{id} => member healthrecord 
        #endregion

        #region Create member
        // Get :: baseuel / member / create => show empty form to create member
        // post :: baseurl/member/create => create member and redirect to index {submit form}
        #endregion

        #region Edit
        // Get :: baseurl/member/edit/{id} => show form with member data to edit
        // post :: baseurl/member/edit/{id} => edit member and redirect to index {submit form}

        #endregion

        #region Delete
        // Get :: baseurl/member/delete/{id} => show validation form to delete member
        #endregion


    }
}
