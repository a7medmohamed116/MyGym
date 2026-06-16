using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IGenericRepository<Member> _MemberRepo;

        public MemberService(IGenericRepository<Member> member_repo)
        {
            _MemberRepo = member_repo;
        }

        public Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            // check email and phone exist or not if exist return false cause we said they should be unique
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var members = await _MemberRepo.GetAllAsync(ct: ct);
            // get all comes from datebase 

            if (!members.Any()) return []; // to handle null exception 
            // take date and send it to view model

            List<MemberViewModel> membervm = new List<MemberViewModel>();

            foreach (var member in members)
            {
                var MemberViewModel = new MemberViewModel()
                {

                    Name = member.Name,
                    Phone = member.Photo,
                    Email = member.Email,
                    Photo = member.Photo,
                    Id = member.Id,
                    Gender = member.Gender.ToString()

                };
                membervm.Add(MemberViewModel);



            }
            return membervm;


        }
    }
}
