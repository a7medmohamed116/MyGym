using AutoMapper;
using GymManagement.BLL.Commn;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.Services.ViewModels.TrainerViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateTrainerAsync(CreateTrianerViewModel model, CancellationToken ct = default)
        {
            var emailexist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(X=>X.Email == model.Email) ;
            var phoneexist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(X=>X.Phone == model.Phone) ;
            if (emailexist) return Result.Validation("Email Already Exist");
            if (phoneexist) return Result.Validation("Phone Number Already Exist");
            var mapped = _mapper.Map<Trainer>(model);
            _unitOfWork.GetRepository<Trainer>().AddAsync(mapped);
             var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Add Trainer");


        }

        public async Task<Result> DeleteTrainer(int trainerid)
        {
            var sessioncheck = await _unitOfWork.SessionRepository.AnyAsync(s => s.TrainerId == trainerid && s.EndDate > DateTime.Now);
            if (sessioncheck) return Result.Fail("Can't Delete Trainers With Active Session");
            var trainr = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid);
            if (trainr is null) return Result.NotFound("Trainer Not Found");
            _unitOfWork.GetRepository<Trainer>().DeleteAsync(trainr);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Trainer");
                
            
        }

        public async Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var Trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            if (!Trainers.Any()) return Result<IEnumerable<TrainerViewModel>>.NotFound("Trainers Not Found");
            var mapped = _mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
            return Result<IEnumerable<TrainerViewModel>>.OK(mapped);


        }

        public async Task<Result<TrainerViewModel>> GetTrainerDetailsByIdAsync(int tranerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(tranerId, ct);
            if (trainer is null) return Result<TrainerViewModel>.NotFound("Trainer Not Found");
            var mapped = _mapper.Map<TrainerViewModel>(trainer);
            return Result<TrainerViewModel>.OK(mapped);
        }

        public async Task<Result<UpdateTrainerViewModel>> GetTrainerToUpdate(int trainerid, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid);
            if (trainer is null) return Result<UpdateTrainerViewModel>.NotFound("Trainer Not Found");
            var mapped = _mapper.Map<UpdateTrainerViewModel>(trainer);
            return Result<UpdateTrainerViewModel>.OK(mapped);


        }

        public async Task<Result> UpdateTrainerAsync(int trainerid, UpdateTrainerViewModel model, CancellationToken ct = default)
        {

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid);
            if (trainer is null) return Result.NotFound("Trainer Not Found");
            var emailexist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(X => X.Email == model.Email && X.Id != trainerid);
            var phoneexist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(X => X.Phone == model.Phone && X.Id != trainerid);
            if (emailexist) return Result.Validation("Email Already Exist");
            if (phoneexist) return Result.Validation("Phone Number Already Exist");
            _mapper.Map(model, trainer);
            _unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Trainer");
        }
    }
}
