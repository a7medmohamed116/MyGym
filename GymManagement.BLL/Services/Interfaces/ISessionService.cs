using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        // get trainers for viewbag  
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDown(CancellationToken ct = default);
        // get Categories for viewbag  
        Task<IEnumerable<CategorySelectViewModel>> GetCategoryrForDropDown(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int sessionid, CancellationToken ct = default);

        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionid, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int sessionid, UpdateSessionViewModel model, CancellationToken ct = default); 
    } 
}
