using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MyGym.PL.Controllers
{
    public class MemberController : Controller
    {


        //MemberService
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MemberController(IMemberService memberService , IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }


        #region Get members
        // Get :: baseurl/member/index => all members

        public async Task<IActionResult> Index( CancellationToken ct = default)
        {

            var members = await _memberService.GetAllAsync(ct);
            return View(members);
        }

        // Get :: baseurl/member/details/{id} => member details  specific member

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct = default)
        {
            var member =await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }

            return View(member);


        }



        // get :: baseurl/member/HealthsRecordDetalis/{id} => member healthrecord 


        public async Task<IActionResult> HealthRecordDetails(int id )
        {

            var record = await _memberService.GetMemberHealthRecordByIdAsync(id);
            if (record is null)
            {
                TempData["ErrorMessage"] = "No Health Record Found !";
                return RedirectToAction(nameof(Index));

            }
            return View(record);
        }


        // get memberphoto 
        [HttpGet]
        public async Task<IActionResult>Picture(int id, CancellationToken ct = default)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null || string.IsNullOrWhiteSpace(member.Photo)) return NotFound();
            var result =  _attachmentService.GetFile(member.Photo, "MembersPhoto");
            if (result is null) return NotFound();
            return File(result.Value.stream, result.Value.ContantType);
        }




        #endregion

        #region Create member
        // Get :: baseuel / member / create => show empty form to create member
        [HttpGet]
        public IActionResult Create() => View();  


        // post :: baseurl/member/create => create member and redirect to index {submit form}

        public async Task<IActionResult> CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {

            //check model state 
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model); // back in form and pass model keep inputed data
            }
            var result = await _memberService.CreateMemberAsync(model, ct); // done
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild to Create Member!";

            } // send the data from controller to view using TempData to show message in index page
            return RedirectToAction(nameof(Index));

        }
        // CreateMember
        #endregion

        #region Edit
        // Get :: baseurl/member/edit/{id} => show form with member data to edit
        [HttpGet]
        public async Task<IActionResult> EditMember(int id , CancellationToken ct =default)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }


        // post :: baseurl/member/edit/{id} => edit member and redirect to index {submit form}

        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id ,MemberToUpdateViewModel model ,CancellationToken ct = default) //[FromRoute] to be safe force it from to safe trom user inspact
        {
            //check model state
            if (!ModelState.IsValid) return View(nameof(EditMember),model);
            var result = await _memberService.UpdateMemberAsync(id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Member";
            }
            return RedirectToAction(nameof(Index));


        }

        #endregion

        #region Delete
        // Get :: baseurl/member/delete/{id} => show validation form to delete member

        public async Task<IActionResult> Delete(int id , CancellationToken ct = default)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(); // show conformation page no need show data so no need view model so no need service so not use @model *** in the view page
        }


        //post :: baseurl/member/DeleteConfirmed
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id ,CancellationToken ct = default)//[FromRoute] to be safe force it from to safe trom user inspact
        {
            var member = await _memberService.DeleteMemberAsync(id, ct);
            if (member)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member";
            }


            return RedirectToAction(nameof(Index));
        }


        #endregion


    }
}
