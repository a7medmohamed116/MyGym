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
        
        
        private readonly IGenericRepository<Plan> _PlanRepository; // will remove IPlanRepository and add IGenericRepository
        public PlanController(IGenericRepository<Plan> planRepository)
        {
            _PlanRepository = planRepository;
        }//this will kill new , no direct deal with connection ,just tell the relaton in program.cs and asp.net handel DI
        //with builder.Services.AddScoped<IPlanRepository, PlanRepositry>();//DI 


        // GET: BaseURL/(controller)Plan/(Action)Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans = await _PlanRepository.GetAllAsync(ct:ct); // pass by name
            return View(Plans);
        }

        // GET: BaseURL/(controller)Plan/(Action)Details/{id}

        public async Task<IActionResult> Deatils(int Id, CancellationToken ct) 
        {
            var plan = await _PlanRepository.GetByIdAsync(Id,ct);

            if (plan is null) return RedirectToAction(nameof(Index));
            return View(plan);

        }
    }
}
