using AutoMapper;
using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, Customer>()
               .ForMember(dest => dest.ProfileType, opt => opt.MapFrom(src => ProfileType.Customer))
               .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<AddFreelancerBusinessDto, Freelancer>()
            .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<Freelancer, FreelancerBusinessResult>();
            
             CreateMap<AddReservationBusinessDto, ReservationProvider>()
            .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<ReservationProvider, ReservationBusinessResult>();

            CreateMap<Customer, CurrentProfileResult>();
            


        }
    }
}
