using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        #region Old pattern
        //private readonly IGenericRepository<Member> _MemberRepo;
        //private readonly IGenericRepository<Membership> _membership_Repo;
        //private readonly IGenericRepository<Plan> _plan_Repo;
        //private readonly IGenericRepository<HealthRecord> _healthrecord_Repo;
        //private readonly IGenericRepository<Booking> _booking_Repo;

        //public MemberService(IGenericRepository<Member> member_repo 
        //    , IGenericRepository<Membership> membership_repo
        //    ,IGenericRepository<Plan> plan_repo
        //    ,IGenericRepository<HealthRecord> healthrecord_repo
        //    ,IGenericRepository<Booking>booking_repo)
        //{
        //    _MemberRepo = member_repo;
        //    _membership_Repo = membership_repo;
        //    _plan_Repo = plan_repo;
        //    _healthrecord_Repo = healthrecord_repo;
        //    _booking_Repo = booking_repo;
        //}
        #endregion

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // we will use unit of work pattern to save changes in one place and not in every repository****************
        //---------UnitOfwork
        public MemberService(IUnitOfWork unitOfWork , IMapper mapper) // mapper to auto map
        {
            _unitOfWork = unitOfWork; // do not forget to register it in program.cs
            _mapper = mapper; // do not forget to register it in program.cs [about each method need from to so make profile and register profile in program.cs] => Folder Profiels =>MappingProfile
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model
            , CancellationToken ct = default)
        {
            // check email and phone exist or not if exist return false cause we said they should be unique
            // go to IGeneric repository and add new method to check if email or phone exist or not [old]"_MemberRepo"
            //[new] _unitOfWork.GetRepository<Member>() first will create new instance of member repo then will use it cause will exist in dictionary of unit of work and will be disposed after the end of the request
            var EmailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Email == model.Email);
            var PhoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Phone == model.Phone);
            if (EmailExist || PhoneExist) return false;
            //Add member 
            // take the member from form { create member view model} and add it in real mmeber entity of data base
            var member = _mapper.Map<CreateMemberViewModel, Member>(model);

           
            //var result = await _MemberRepo.AddAsync(member);
            //return result > 0; [old]

            //[new]
            _unitOfWork.GetRepository<Member>().AddAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

        public async Task<bool> DeleteMemberAsync(int memberid, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberid, ct);
            if (member is null) return false;

            //check if has active booking or no // so register booking  table above
            var hasactivebooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(X => X.MemberId == memberid && X.Session.StartDate < DateTime.Now); //upcoming session !!Exception  
            if (hasactivebooking) return false;

            _unitOfWork.GetRepository<Member>().DeleteAsync(member, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
            
                
           
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            // get all comes from datebase 

            if (!members.Any()) return []; // to handle null exception 
            // take date and send it to view model

            List<MemberViewModel> membervm = new List<MemberViewModel>();

            foreach (var member in members)
            {
                var MemberViewModel = new MemberViewModel()
                {

                    Name = member.Name,
                    Phone = member.Phone,
                    Email = member.Email,
                    Photo = member.Photo,
                    Id = member.Id,
                    Gender = member.Gender.ToString()

                };
                membervm.Add(MemberViewModel);



            }
            return membervm;


        }

        
        
        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int memberid, CancellationToken ct = default)
        {
            //get member by id
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberid, ct);
            if (member == null) return null;
            // pass the data from member to view model


            //var model = new MemberViewModel()
            //{

            //    Name = member.Name,
            //    Phone = member.Phone,
            //    Email = member.Email,
            //    Photo = member.Photo,
            //    Gender = member.Gender.ToString(),
            //    DateOfBirth = member.DareOfBirth.ToShortDateString(),
            //    Address = $"{member.Address.BuildingNumber} _ {member.Address.Street} _ {member.Address.City}",
            //    // plan name ?????????
            //    // membership start & end date  ?????????

            //};[old]

            //[new]
            //var model = _mapper.Map<Member, MemberViewModel>(member); or
            var model = _mapper.Map<MemberViewModel>(member);

            // check if user has Active MemberShip?? =>  plan or not 
            // will need membership table so will register it above  in IGeneric 
            var ActiveMemberShip = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(X => X.MemberId == memberid && X.EndDate > DateTime.Now);
            if (ActiveMemberShip is not null)
            {
                //plannamw?? so will register it above in IGeneric
                var activeplan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMemberShip.PlanId, ct);
                model.PlanName = activeplan?.Name;
                model.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToString();
                model.MemberShipEndDate = ActiveMemberShip.EndDate.ToString();
            }

            return model;


        }

        public async Task<HealthRecordViewModel> GetMemberHealthRecordByIdAsync(int memberid, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(X => X.MemberId == memberid, ct: ct);
            if (record is null) return null;
            else
                // health record to healthrecordviewmodel
                return _mapper.Map<HealthRecord,HealthRecordViewModel>(record);// do u have profile for this ? no => go first mark in profile
                
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberid, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberid, ct);
            if (member is null) return null;
            else
                // from member to membertoupdateviewmodel 
                return _mapper.Map<MemberToUpdateViewModel>(member);           
                
            


        }

        public async Task<bool> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            //get mmeber
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            //check the if any onther member has phone or email
            var emailexist = await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Email == model.Email && X.Id != id); //not the current member cause may the same one save the same email while editing
            var phoneexist = await _unitOfWork.GetRepository<Member>().AnyAsync(X => X.Phone == model.Phone && X.Id != id);
            if (emailexist || phoneexist) return false;
            //membertoupdateviewmodel to member
            //  _mapper.Map<Member>(member); wrong => create new object the address is null here
            _mapper.Map<MemberToUpdateViewModel,Member>(model,member);
            member.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
            
                
            

        }



        #region From Manual Map to Auto Mapper
        //Download Auto Mapper In BLL 
        //inject it in ctor above
        //[about each method need {from} {to} so make profile and use it's implementaion in progrm.cs {.AddAutoMapper(X=>X.AddProfile(new mappingprofile))}] => Folder Prfiels in BLL => MappingProfile inherited {Profile}=> belongs to AutoMapper 
        // after it add each mapp in profile
        // the flow is => mapper in ctor above => So will go to program.cs Any Resgisterd map Auto ? yes Addprofile use mapping profile
        // which mapping profile contains every map i put in it and need!
        // map by confnuition   
        // set in every method/service =>   var model = _mapper.Map<MemberViewModel>(member) ; etc
        #endregion
    }
}
