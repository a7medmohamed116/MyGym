using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MyGym.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await _trainerService.GetAllTrainersAsync(ct);
            if (result.success)
            {
                return View(result.Value);
            }
            TempData["ErrorMessage"] = result.error;
            return View(Enumerable.Empty<TrainerViewModel>());
            
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrianerViewModel model , CancellationToken ct =default) 
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;
            return View(model);


        }

        public async Task<IActionResult>Details(int id ,CancellationToken ct =default)
        {
            var result = await _trainerService.GetTrainerDetailsByIdAsync(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
            }
            return View(result.Value);

        }
        public async Task<IActionResult> Edit(int id , CancellationToken ct =default)
        {
            var trainer = await _trainerService.GetTrainerToUpdate(id, ct);
            if (trainer.success)
            {
                return View(trainer.Value);
            }
            TempData["ErrorMessage"] = trainer.error;
            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        public async Task<IActionResult>Edit([FromRoute]int id , UpdateTrainerViewModel model ,CancellationToken ct = default)
        {

            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _trainerService.UpdateTrainerAsync(id, model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _trainerService.GetTrainerDetailsByIdAsync(id);
            if (trainer is null)
            {
                return NotFound();

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id)
        {
            var result = await _trainerService.DeleteTrainer(id);
            if (result.success)
            {
                TempData["Successmessage"] = "Trainer Deleted Successfully";
                return RedirectToAction(nameof(Index));

            }
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }



    }
}
