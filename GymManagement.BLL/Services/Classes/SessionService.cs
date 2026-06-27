using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork ,IMapper mapper) // unitofwork don't no thing about sessionrepo so register it in unitofwork and use it here
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            #region Info About Validations rules logic
            //Validations => bussinus rules logic while view models => ui validation (work in !isvalid.successd in controller) 
            // the service can be called without mvc from [api ,unit test , background job ,console app] so in this case data annotations won't work
            // aslo no data annotations for rule like this EndDate > StartDate
            // aslo if in domain someone send var model = new CreateSessionViewModel
            //{
            //  Capacity = -100
            //}
            // !!!!!!!!!!!!!!!!!!هتدخل بيانات فاسدة للداتابيز.
            // with exist validation in service this make it the last gard to safe data
            #endregion


            if (model.EndDate <= model.StartDate)  return Result.Validation("EndDate Must Be Greater Than StartDate");
            if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate Must Be In The Future");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity Must Be Between 1 And 25");
            
            // get trainer
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found!");
            //get category
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.NotFound("Category Not Found!");
            //check if trainer specaillty == category specaillty
            var isvalid = Enum.TryParse<Specialty>(category.CategoryName, true,out var specailtyresult);

            if (!isvalid || trainer.Specialty != specailtyresult) return Result.Validation("Trainer And Category  Must Be The Same speciality");
            
            // createsessionviewmodel to session
            var session = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().AddAsync(session); // will use a normal add can with getrepo or by sessionrepo.add casuse it inherieted the generic اصلا
            var result = await _unitOfWork.SaveChangesAsync(ct);
            //return result > 0; [old]
            return result > 0 ? Result.OK() : Result.Fail("Failed To Create Session"); 





        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            //**Trainer , Category // new method represent session with trainer and category
            //when load table session  give me also table trainer and category with it
            // loading [lazy] or [eager] => Include or [explicit] === will use eager loading with Include method
            // can't implement method in IGenericRepository because it is not generic method, it is specific to session entity
            // so we will implement it with ISessionRepository
            // after implement and connect with generic and Unit of work doooooooone
            // **need count of peapole booked the session => new method in ISessionRepo
            //*************************************
            // var sessions = await _unitOfWork.GetRepository<Trainer>().indclude??????
            //var booked =  _unitOfWork.GetRepository<Booking>().count 
            //ليه روحت لفيت وعملتهمم ميثودس في السيشن ريبو عشاتن لو جربت اعمل كدا هفشل لان يونت اوف ورك انا  اللي عاملوا
            // ف طبيعي محدود ب ميثودش معينه مش هلاقي فيه ال انكلود او ال كاونت
            // ف السيشن ريبو بكلم ديبي كونتيكست ف معايا اكسيس للحاجات الجميله دي 
            var sessions = await _unitOfWork.SessionRepository.GetSessionsWithTrainerAndCategory(ct);//.sessionrepo => properity
            if (sessions is null || !sessions.Any()) return null;
            // map sessions to session view model



                var mappedsession = sessions.Select(S => new SessionViewModel() //select retuen manysessions
                {
                    Id = S.Id,
                    Capacity = S.Capacity,
                    CategoryName = S.Category.CategoryName,
                    TrainerName = S.Trainer.Name,
                    StartDate = S.StartDate,
                    EndDate = S.EndDate,
                    Description = S.Description
                    
                    

                });// ممكن Select نفسه async ونستخدم Task.WhenAll. الأخير غالبًا أفضل
            foreach (var session in mappedsession)
            {
                // availableslots => capcity - count
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(session.Id, ct);
            }//need await so async , the above values are ready , so calculate it sperately

            
            return mappedsession;

            









        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryrForDropDown(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct:ct);
            return  _mapper.Map<IEnumerable <CategorySelectViewModel>>(categories); // map of many use IEnummerable 
            
        }

        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int sessionid, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionid, ct);
            if (session is null)
            {
                return Result<SessionViewModel>.NotFound("Session Not Found");
            }
            // map for session to sessionviewmodel
            var mappedsession = _mapper.Map<SessionViewModel>(session);
            // map avaliable slots manual
            mappedsession.AvailableSlots = mappedsession.Capacity - await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(sessionid, ct);
            return Result<SessionViewModel>.OK(mappedsession);  
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionid, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionid, ct);
            if (session is null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");
            if (session.EndDate < DateTime.Now) return Result<UpdateSessionViewModel>.Fail("Cannot Update Completed Session !");
            if (session.StartDate <= DateTime.Now) return Result<UpdateSessionViewModel>.Fail("Cannot Update Ongoing Session !");
            
            // cannot update session 
            var bookingcount = await _unitOfWork.SessionRepository.CountOfBookedSlotsAsync(sessionid, ct);
            if (bookingcount >0) Result<UpdateSessionViewModel>.Fail("Cannot Update Session Already Booked ");
            var mappedsession = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.OK(mappedsession);

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDown(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<Result> UpdateSessionAsync(int sessionid, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionid, ct);
            if (session is null) return Result.NotFound("Session Not Found");

            if (model.StartDate < DateTime.Now) return Result.Validation("Start Date Must Be In The Future");
            if (model.StartDate > model.EndDate) return Result.Validation("EndDate Must Be After StartDate");


            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);//model {changable}
            if (trainer is null) return Result.NotFound("Trainer Not Found");
          
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);//session from data base

            var Isvalid = Enum.TryParse<Specialty>(category?.CategoryName,true, out var categotyseciallty);
            if (!Isvalid && trainer.Specialty != categotyseciallty) return Result.Validation("Category And Trainer Must Be The Same Speciallty");
             _mapper.Map<UpdateSessionViewModel, Session>(model, session); // for reverse map
            session.UpdatedAt = DateTime.Now; 
            _unitOfWork.SessionRepository.UpdateAsync(session);
            var result =  await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Session");             

        }
    }
}
