using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyGym.PL.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService; //register in program.cs <ISessionService,SessionSrevice>
        }

        //GEt :: BaseUrl/Session/Index
        public async Task<IActionResult> Index(CancellationToken ct =default)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
                 
            
            return View(sessions);
        }
        // get :: baseurl/session/create
        [HttpGet]
        public async Task<IActionResult> Create()

        {
            //send the data from cotroller to the view using viewbag
            await DropDownList();
            return View(); //view  of empty form 

        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateSessionViewModel  model ,CancellationToken  ct = default)
        {
            //check modelstate
            if (!ModelState.IsValid) {

                //if there is a problem will retuen me to view without load them so must call here
                await DropDownList();
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model);
            if (result.success)//[after result pattern] {.success}
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return  RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error; //.error [new] after result pattern
            await DropDownList();
            return View(model);


        } 

        private async Task DropDownList()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDown(), "Id", "Name");// from what ,which value , what persentaion
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoryrForDropDown(), "Id", "CategoryName");
        }


        public async Task<IActionResult> Details(int id , CancellationToken ct =default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (result.success)
            {
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id ,CancellationToken ct =default)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (result.success)
            {
                await DropDownList();
                return View(result.Value);
            }

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, UpdateSessionViewModel model ,CancellationToken ct = default)
        {

            if (!ModelState.IsValid)
            {
                await DropDownList();
                return View(model);
            }


            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                await DropDownList();
                return View(model);
            }
        }
            


        public async Task<IActionResult> Delete(int id , CancellationToken ct =default)
        {
            var session = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (!session.success)
            {
                TempData["ErrorMessage"] = session.error;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct =default)
        {

            var result = await _sessionService.DeleteSession(id ,ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
                
            }
            else {
                TempData["ErrorMessage"] = result.error;
            }
            return RedirectToAction(nameof(Index));

        }




    }
}
