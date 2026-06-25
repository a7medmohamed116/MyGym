using AutoMapper;
//using AutoMapper.Execution;
using GymManagement.BLL.Services.ViewModels.MemberViewModels;
using GymManagement.BLL.Services.ViewModels.PlanViewModels;
using GymManagement.BLL.Services.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Profiels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            SessionProfiels();
            MemberProfiels();
            PlanProfiels();

        }

        private void SessionProfiels()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Session, SessionViewModel>()
                                                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                                                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                                 .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());//will be calculated after map
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            
        }
        

        private void MemberProfiels()
        {
            //has problem with objects like address so need to define it and {Birhdate} cause i say .toshortdatestring 
            CreateMap<Member, MemberViewModel>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} _ {src.Address.Street} _ {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DareOfBirth.ToShortDateString()));// from  member to member view model
            // (formember)dest => member view model , src => member

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();  // health record to healthrecordviewmodel // reverse رايحة جاي
            CreateMap<Member, MemberToUpdateViewModel>()
                                                        .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(scr => scr.Address.BuildingNumber))
                                                        .ForMember(dest => dest.City, opt => opt.MapFrom(scr => scr.Address.City))
                                                        .ForMember(dest => dest.Street, opt => opt.MapFrom(scr => scr.Address.Street));// from member to membertoupdateviewmodel 

            CreateMap<MemberToUpdateViewModel, Member>()
                                                    .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                                                    .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                                                    .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                                                    .ForMember(des => des.Name, opt => opt.Ignore())
                                                    .ForMember(des => des.Photo, opt => opt.Ignore());//membertoupdateviewmodel to member // name photo must tell him ignore them no need to map cause they read only don't touch them
                                                                                                      //formember take a independent object so use *forpath* or aftermapper


            #region bad way from creatememberviewmodel to member

            //CreateMap<CreateMemberViewModel, Member>()
            //                                        .ForPath(dest=> dest.Address.BuildingNumber , opt => opt.MapFrom(src=>src.BuildingNumber ))
            //                                        .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
            //                                        .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
            //                                        .ForPath(dest => dest.HealthRecord.BloodType, opt => opt.MapFrom(src => src.HealthRecordViewModel.BloodType))
            //                                        .ForPath(dest => dest.HealthRecord.Height, opt => opt.MapFrom(src => src.HealthRecordViewModel.Height))
            //                                        .ForPath(dest => dest.HealthRecord.Weight, opt => opt.MapFrom(src => src.HealthRecordViewModel.Weight))
            //                                        .ForPath(dest => dest.HealthRecord.Note, opt => opt.MapFrom(src => src.HealthRecordViewModel.Note));// from creatememberviewmodel to member 
            //this way if i have object and want to update and control every prop self too long so will use hossam's way
            #endregion


            // creatememberviewmodel to member
            CreateMap<CreateMemberViewModel, Member>()
                                                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                                                    {
                                                        BuildingNumber = src.BuildingNumber,
                                                        Street = src.Street,
                                                        City = src.City,
                                                    }))
                                                    .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel)); //if had this map will use it
                                                                                                                                                 // more simple and cause create new object and if i have old map will use it


        }

        private void PlanProfiels()
        {
            CreateMap<Plan, PlanViewModel>();
        }
    }
} 
