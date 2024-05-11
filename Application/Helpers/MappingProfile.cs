﻿    using AutoMapper;
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
          

            CreateMap<Freelancer, FreelancerProfileResult>();

            CreateMap<Freelancer, FreelancerFilterResultDto>();
            
             CreateMap<AddReservationBusinessDto, ReservationProvider>()
            .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<ReservationProvider, ReservationBusinessResult>();

            CreateMap<Customer, CustomerProfileResult>();

            CreateMap<AddOfferedServiceDto, OfferedService>()//remove date of join when update
          .ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));


            CreateMap<OfferedService, OfferedServiceResult>();


            CreateMap<SendServiceRequestDto, ServiceRequest>()
              .ForMember(dest => dest.DateTime, opt => opt.MapFrom(_ => DateTime.Now))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RequisStatus.Pending));

            CreateMap<ServiceRequest, CustomerServiceRequestResult>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ServiceRequest, FreelancerServiceRequistResult>()
         .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));



            CreateMap<RequestedCustomerDto, Customer>();
            CreateMap<RequestedFreelancerDto, Freelancer>();




             
        }
    }
}
