using AutoMapper;
using SyncHRoner.Domain.Entities;
using SyncHRoner.Domain.Enums;
using SyncHRoner.Domain.ValueObjects;
using SyncHRoner.Dtos;
using SyncHRoner.Dtos.CreateDtos;
using SyncHRoner.Dtos.UpdateDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncHRoner.Automapper
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ProfileCreateDto, SyncHRoner.Domain.Entities.Profile>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(x => FullName.Create(x.FirstName, x.LastName).GetSuccessValue()))
                .ForMember(dest => dest.BirthDate,
                           opt => opt.MapFrom(x => BirthDate.Create(x.BirthDate).GetSuccessValue()))
                .ForMember(opt => opt.Phone,
                           dest => dest.MapFrom(x => Phone.Create(x.Phone).GetSuccessValue()))
                .ForMember(dest => dest.GenderId,
                            opt => opt.MapFrom(x => (long)x.Gender))
                .ForMember(opt => opt.Email,
                           dest => dest.MapFrom(x => Email.Create(x.Email).GetSuccessValue()))
                .ForMember(opt => opt.Rating,
                           dest => dest.MapFrom(x => Rating.Create(x.Rating).GetSuccessValue()))
                .ForMember(opt => opt.IsActive,
                           dest => dest.MapFrom(x => x.IsActive)).AfterMap((dto, obj) =>
                           {
                               foreach(var item in dto.Nationalities)
                               {
                                   obj.AddNationality((long)item);
                               }
                           });

            CreateMap<ProfileUpdateDto, SyncHRoner.Domain.Entities.Profile>()
                 .ForMember(dest => dest.FullName,
                            opt => opt.MapFrom(x => FullName.Create(x.FirstName, x.LastName).GetSuccessValue()))
                 .ForMember(dest => dest.BirthDate,
                            opt => opt.MapFrom(x => BirthDate.Create(x.BirthDate).GetSuccessValue()))
                 .ForMember(dest => dest.GenderId,
                            opt => opt.MapFrom(x => (long)x.Gender))
                 .ForMember(opt => opt.Phone,
                            dest => dest.MapFrom(x => Phone.Create(x.Phone).GetSuccessValue()))
                 .ForMember(opt => opt.Email,
                            dest => dest.MapFrom(x => Email.Create(x.Email).GetSuccessValue()))
                 .ForMember(opt => opt.Rating,
                            dest => dest.MapFrom(x => Rating.Create(x.Rating).GetSuccessValue()))
                 .ForMember(opt => opt.LastUpdate,
                            dest => dest.MapFrom(x => x.LastUpdate)).AfterMap((dto, obj) =>
                            {
                                foreach (var item in dto.Nationalities)
                                {
                                    obj.AddNationality((long)item);
                                }
                            });


            CreateMap<SyncHRoner.Domain.Entities.Profile, ProfileDto>()
                .ForMember(dest => dest.Id,
                             opt => opt.MapFrom(x => x.Id))
                  .ForMember(dest => dest.FirstName,
                            opt => opt.MapFrom(x => x.FullName.FirstName))
                  .ForMember(dest => dest.LastName,
                            opt => opt.MapFrom(x => x.FullName.LastName))
                 .ForMember(dest => dest.BirthDate,
                            opt => opt.MapFrom(x => x.BirthDate.Value))
                 .ForMember(dest => dest.Gender,
                            opt => opt.MapFrom(x => (GenderEnum)x.GenderId))
                 .ForMember(opt => opt.Phone,
                            dest => dest.MapFrom(x => x.Phone.Value))
                 .ForMember(opt => opt.Email,
                            dest => dest.MapFrom(x => x.Email.Value))
                 .ForMember(opt => opt.Rating,
                            dest => dest.MapFrom(x => x.Rating.Value)).AfterMap((obj, dto) =>
                            {
                                foreach(var item in obj.NationalitiesLink)
                                {
                                    dto.Nationalities.Add((CountryEnum)item.CountryId);
                                }
                            });
        }
    }
}
