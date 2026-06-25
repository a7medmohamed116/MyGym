using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGym.Context;
using MyGym.Models;
using System.Numerics;

namespace MyGym.Controllers
{
    public class PlanController : Controller
    {


        //Database context will be here Connection and pass it to cotr
        //[new] will remove the connectiom from here and put it in the repository and inject the repository here
        //private readonly GymDbContext _context;
        //[new] no need ctor too
        //public PlanController()
        //{
        //    _context = new GymDbContext();
        //}
        //private readonly IPlanRepository PlanRepository = new PlanRepositry();
        // still have problem with DI


        //private readonly IGenericRepository<Plan> _PlanRepository; // will remove IPlanRepository and add IGenericRepository
        //public PlanController(IGenericRepository<Plan> planRepository)
        //{
        //   _PlanRepository = planRepository;
        //}//this will kill new , no direct deal with connection ,just tell the relaton in program.cs and asp.net handel DI
        //with builder.Services.AddScoped<IPlanRepository, PlanRepositry>();//DI 


        //[new]   services
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService)
        {
            _planService = planService; // refister di in program.cs
        }


        // GET: BaseURL/(controller)Plan/(Action)Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans = await _planService.GetAllPlansAsync(ct);
            
            
            return View(Plans);
            

        }

        //GET: BaseURL/(controller) Plan/(Action) Details/{id}

        public async Task<IActionResult> Deatils(int Id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanDetailsByIdAsync(Id, ct);

            if (!plan.success) return NotFound(); // not plan view model it's Result<PlanViewModel>
            return View(plan.Value); // took the planview model and send it to view {value}

        }

        //GET :: plan/Activate/{id}
        public async Task<IActionResult> Activate(int id ,CancellationToken ct =default)
        {
            var plan = await _planService.ActivateButtom(id, ct);
            return RedirectToAction(nameof(Index));


        }

    }
}
