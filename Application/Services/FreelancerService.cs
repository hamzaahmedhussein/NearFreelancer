using AutoMapper;
using Castle.Core.Resource;
using Connect.Application.Consts;
using Connect.Application.DTOs;
using Connect.Application.Consts;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Connect.Core.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

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

            var freelancer = currentUser.Freelancer;

            _unitOfWork.CreateTransaction();
            
            var image = await _userHelpers.AddImage(serviceDto.Image, Consts.Consts.Freelancer);
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
                await _userHelpers.DeleteImageAsync(image,Consts.Consts.Freelancer);
                return false;
            }
        }

        public async Task<bool> UpdateOfferedService(string id, AddOfferedServiceDto serviceDto)
        {
            var offeredService = _unitOfWork.OfferedService.GetById(id);
            var currentUser = await _userHelpers.GetCurrentUserAsync();

            if (currentUser.Freelancer != null)
            {
                try
                {
                    _unitOfWork.CreateTransaction();
                    var image = await _userHelpers.UpdateImageAsync(serviceDto.Image,offeredService.Image , Consts.Consts.Freelancer);
                    offeredService = _mapper.Map<OfferedService>(serviceDto);
                    offeredService.Image = image;
                    _unitOfWork.OfferedService.Update(offeredService);
                    _unitOfWork.Save();
                
                    _unitOfWork.Commit();
                    return true;
                }
                catch
                {
                    _unitOfWork.Rollback();
                    //await _userHelpers.DeleteImageAsync(image, Consts.Consts.Freelancer);
                    return false;
                }
            }
            return false;
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
            var freelancer = customer.Freelancer;
            if (customer == null)
                    throw new Exception("User not found");

                if (await _userManager.IsInRoleAsync(customer, "Freelancer") == false)
                    throw new Exception("User not freelancer");

            System.Console.WriteLine(freelancer.Id);
            var requests = await _unitOfWork.ServiceRequest.FindAsync(r=>r.FreelancerId == freelancer.Id);

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

        public async Task<bool> UpdateFreelancerBusiness(AddFreelancerBusinessDto freelancerDto)
        {
            var user=await _userHelpers.GetCurrentUserAsync();
            var oldFreelancerBusiness = user.Freelancer;
            
            try
            {
                oldFreelancerBusiness = _mapper.Map<Freelancer>(freelancerDto);
                _unitOfWork.FreelancerBusiness.Update(oldFreelancerBusiness);
                _unitOfWork.Save();
                return true;
            }
            catch
            {
                
                return false;
            }
        }

        public async Task<bool> DeleteFreelancerBusinessAsync()
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var user = await _userHelpers.GetCurrentUserAsync();
                await _userManager.RemoveFromRoleAsync(user, "Freelancer");
                var freelancer = user.Freelancer;
                var services = freelancer.OfferedServicesList.ToList();
                _unitOfWork.FreelancerBusiness.Remove(user.Freelancer);
                if (_unitOfWork.Save() > 0)
                    foreach (var service in services)
                        await _userHelpers.DeleteImageAsync(service.Image, Consts.Consts.Freelancer);
                _unitOfWork.Commit();
                return true;
            }catch
            {
                _unitOfWork.Rollback();
                return false;
            }
        }
    } }


