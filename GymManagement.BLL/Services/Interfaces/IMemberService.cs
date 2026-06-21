using GymManagement.BLL.Services.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        // get all members
        Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken ct = default);
        // Create a new member
        Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);
        //get member details by id
        Task<MemberViewModel?> GetMemberDetailsByIdAsync(int memberid, CancellationToken ct = default);
        // get member health record by id
        Task<HealthRecordViewModel> GetMemberHealthRecordByIdAsync(int memberid, CancellationToken ct = default);
        //get member to update
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberid, CancellationToken ct = default);

        //update member
        Task<bool> UpdateMemberAsync(int id,MemberToUpdateViewModel model , CancellationToken ct = default);
        // delete member 
        Task<bool> DeleteMemberAsync(int memberid, CancellationToken ct = default);
    }
}
