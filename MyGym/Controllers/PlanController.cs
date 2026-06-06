using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGym.Context;
namespace MyGym.Controllers
{
    public class PlanController : Controller
    {
        //Database context will be here Connection and pass it to cotr
        private readonly GymDbContext _context;

        public PlanController()
        {
            _context = new GymDbContext();
        }

        // GET: BaseURL/(controller)Plan/(Action)Index
        public async Task<IActionResult> Index()
        {
            var Plans = await _context.Plans.ToListAsync();
            return View(Plans);
        }

        // GET: BaseURL/(controller)Plan/(Action)Details/{id}

        public async Task<IActionResult> Deatils(int Id) 
        {
            var plan = await _context.Plans.FindAsync(Id);

            if (plan is null) return RedirectToAction(nameof(Index));
            return View(plan);

        }
    }
}
