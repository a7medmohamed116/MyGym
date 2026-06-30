using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<Result>CreateTrainerAsync(CreateTrianerViewModel model,CancellationToken ct = default);
        Task<Result<TrainerViewModel>>GetTrainerDetailsByIdAsync(int tranerId , CancellationToken ct =default);
        Task<Result<UpdateTrainerViewModel>> GetTrainerToUpdate(int trainerid, CancellationToken ct = default);
        Task<Result>UpdateTrainerAsync(int trainerid,UpdateTrainerViewModel model, CancellationToken ct = default);
        Task<Result>DeleteTrainer(int trainerid);
    }
}
