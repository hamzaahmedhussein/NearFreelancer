    using AutoMapper;
using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, Customer>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => new MailAddress(src.Email).User))
               .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<UpdateCustomerInfoDto, Customer>();

            CreateMap<AddFreelancerBusinessDto, Freelancer>()
            .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));
          

            CreateMap<Freelancer, FreelancerBusinessResult>();

            CreateMap<Freelancer, FreelancerFilterResultDto>();
            
             CreateMap<AddReservationBusinessDto, ReservationProvider>()
            .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<ReservationProvider, ReservationBusinessResult>();

            CreateMap<Customer, CurrentProfileResult>();

            CreateMap<AddOfferedServiceDto, OfferedService>()//remove date of join when update
          .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));



            CreateMap<SendServiceRequestDto, ServiceRequest>()
              .ForMember(dest => dest.DateTime, opt => opt.MapFrom(_ => DateTime.Now))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RequisStatus.Pending));

            CreateMap<ServiceRequest, GetCustomerRequestsDto>()
           .ForMember(dest => dest.RequestName, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.FreelancerName, opt => opt.MapFrom(src => src.Freelancer.Name)) 
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())); 







        }
    }
}
