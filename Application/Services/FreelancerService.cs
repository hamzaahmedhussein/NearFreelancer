﻿using AutoMapper;
using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Connect.Core.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Connect.Application.Services
{
    public class FreelancerService : IFreelancerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly UserManager<Customer> _userManager;

        public FreelancerService(IUnitOfWork unitOfWork,
            IConfiguration config, IMapper mapper,
            IUserHelpers userHelpers, UserManager<Customer> userManager
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _userManager = userManager;

        }

        public async Task<bool> AddFreelancerBusiness(AddFreelancerBusinessDto freelancerDto)
        {
            if (freelancerDto == null)
                return false;

            var user = await _userHelpers.GetCurrentUserAsync();

            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (await _userManager.IsInRoleAsync(user, "Freelancer"))
                throw new InvalidOperationException("User already has a freelancer profile.");
            try
            {
                _unitOfWork.CreateTransaction();
                var result = await _userManager.AddToRoleAsync(user, "Freelancer");
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to assign Freelancer role to the user.");
                //_unitOfWork.Save();
                var freelancer = _mapper.Map<Freelancer>(freelancerDto);
                freelancer.Owner = user;
                _unitOfWork.FreelancerBusiness.Add(freelancer);
                user.Freelancer = freelancer;
                _unitOfWork.Save();
                _unitOfWork.Commit();
            } catch
            {
                _unitOfWork.Rollback();
                return false;
            }


            return true;
        }



        public async Task<FreelancerBusinessResult> GetFreelancerProfile()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return null;
            var spec = new CustomerWithFreelancer(currentUser.Id);
            var user = await _unitOfWork.Customer.GetByIdWithSpecAsync(spec);
            var freelancer = user?.Freelancer;
            if (freelancer != null)
            {
                return _mapper.Map<FreelancerBusinessResult>(freelancer);
            }

            return null;
        }

        public async Task<FreelancerBusinessResult> GetFreelancerById(string id)
        {
            var profile = _unitOfWork.FreelancerBusiness.GetById(id);
            return _mapper.Map<FreelancerBusinessResult>(profile);
        }

        public async Task<bool> AddOfferedService(AddOfferedServiceDto serviceDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return false;

            var spec = new CustomerWithFreelancer(currentUser.Id);
            var user = await _unitOfWork.Customer.GetByIdWithSpecAsync(spec);
            var freelancer = user.Freelancer;

            _unitOfWork.CreateTransaction();

            var image = await _userHelpers.AddFreelancerImage(serviceDto.Image);
            var offeredService = _mapper.Map<OfferedService>(serviceDto);
            offeredService.Image = image;

            try
            {
                if (freelancer != null)
                {
                    freelancer.OfferedServicesList.Add(offeredService);
                    _unitOfWork.Save();
                }
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                await _userHelpers.DeleteFreelancerImageAsync(image);
                return false;
            }
        }


        public async Task<IEnumerable<FreelancerFilterResultDto>> FilterFreelancers(FilterFreelancersDto filterDto)
            {
                var query = await _unitOfWork.FreelancerBusiness.GetAllAsync();

                if (!string.IsNullOrWhiteSpace(filterDto.Name))
                {
                    query = query.Where(e => e.Name.Contains(filterDto.Name));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Profession))
                {
                    query = query.Where(e => e.Profession.Contains(filterDto.Profession));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.City))
                {
                    query = query.Where(e => e.City == filterDto.City);
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Street))
                {
                    query = query.Where(e => e.Street == filterDto.Street);
                }

                if (filterDto.Skills != null && filterDto.Skills.Any())
                {
                    foreach (var skill in filterDto.Skills)
                    {
                        query = query.Where(e => e.Skills.Contains(skill));
                    }
                }

                query = query.Where(e => e.Availability == filterDto.Availability);

                return _mapper.Map<IEnumerable<FreelancerFilterResultDto>>(query);
            }

            public async Task<IEnumerable<GetCustomerRequestsDto>> GetFreelancerRequests()
            {
                var customer = await _userHelpers.GetCurrentUserAsync();
                if (customer == null)
                    throw new Exception("User not found");

                if (await _userManager.IsInRoleAsync(customer, "Freelancer") == false)
                    throw new Exception("User not freelancer");



                var requests = await _unitOfWork.ServiceRequest.FindAsync(c => c.FreelanceId == customer.Freelancer.Id);

                if (requests == null)
                    throw new Exception("No requests");


                return _mapper.Map<IEnumerable<GetCustomerRequestsDto>>(requests);
            }

            public async Task<bool> AcceptServiceRequest(string requestId)
            {
                var serviceRequest = _unitOfWork.ServiceRequest.GetById(requestId);
                if (serviceRequest == null)
                    throw new Exception("Service request not found.");

                if (serviceRequest.Status != RequisStatus.Pending)
                    throw new Exception("Service request is not in a pending state.");

                serviceRequest.Status = RequisStatus.Accepted;
                _unitOfWork.Save();
                return true;
            }

            public async Task<bool> RefuseServiceRequest(string requestId)
            {
                var serviceRequest = _unitOfWork.ServiceRequest.GetById(requestId);
                if (serviceRequest == null)
                    throw new Exception("Service request not found.");

                if (serviceRequest.Status != RequisStatus.Pending)
                    throw new Exception("Service request is not in a pending state.");

                serviceRequest.Status = RequisStatus.Refused;
                _unitOfWork.Save();
                return true;
            }

        } }


