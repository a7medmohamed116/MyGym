using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using MyGym.Context;
using Microsoft.EntityFrameworkCore;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.Classes;
using AutoMapper;
using GymManagement.BLL.Profiels;
using GymManagement.DAL.Context;
using MyGym.PL;

namespace MyGym
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Register DbContext
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });//DI Will search in Appseting with access options
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Register DI
            builder.Services.AddScoped(typeof(IGenericRepository<> ), typeof(GenericRepository<>));//different way to register generic 
            builder.Services.AddScoped<IMemberService , MemberService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository,SessionRepository>();
            builder.Services.AddScoped<ISessionService,SessionService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<IAttachmentService,AttachmentService>();
            builder.Services.AddScoped<ITrainerService,TrainerService>();
            builder.Services.AddScoped<IMemberShipRepository, MemberSihipRepository>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IBookingRepository,BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            // Add auto mapper
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfile()));

            var app = builder.Build();
            //add seeding 
            await app.MigrateAndSeedDataAsync();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
