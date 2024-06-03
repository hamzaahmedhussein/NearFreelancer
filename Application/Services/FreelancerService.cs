using AutoMapper;
using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Connect.Core.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Connect.Application.Specifications;

namespace Connect.Application.Services
{
    public class FreelancerService : IFreelancerService
    {
        #region Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly UserManager<Customer> _userManager;
        private readonly ILogger<FreelancerService> _logger;


        public FreelancerService(IUnitOfWork unitOfWork,
            IConfiguration config, IMapper mapper,
            IUserHelpers userHelpers, UserManager<Customer> userManager,
            ILogger<FreelancerService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _userManager = userManager;
            _logger = logger;

        }
        #endregion

        public async Task<bool> AddFreelancerBusiness(AddFreelancerBusinessDto freelancerDto)
        {
            if (freelancerDto == null)
                return false;

            var user = await _userHelpers.GetCurrentUserAsync();

            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (await _userManager.IsInRoleAsync(user, "Freelancer"))
                throw new InvalidOperationException("User already has a freelancer profile.");
                _unitOfWork.CreateTransaction();
            try
            {
                var result = await _userManager.AddToRoleAsync(user, "Freelancer");
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to assign Freelancer role to the user.");
                var freelancer = _mapper.Map<Freelancer>(freelancerDto);
                freelancer.Owner = user;
                _unitOfWork.FreelancerBusiness.Add(freelancer);
                user.Freelancer = freelancer;
                _unitOfWork.Save();
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                return false;
            }


            return true;
        }


        public async Task<FreelancerProfileResult> GetFreelancerProfile()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return null;

            var spec = new CustomerWithFreelancerSpec(currentUser.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(spec);

            if (customer != null && customer.Freelancer != null)
            {
                var freelancerResult = _mapper.Map<FreelancerProfileResult>(customer.Freelancer);
                return freelancerResult;
            }

            return null;
        }


        #region GetFreelancerById
        public async Task<FreelancerProfileResult> GetFreelancerById(string id)
        {
            try
            {
                _logger.LogInformation("Getting freelancer profile for ID: {FreelancerId} with offered services", id);


                var profile =  _unitOfWork.FreelancerBusiness.GetById(id);
                if (profile == null)
                {
                    _logger.LogWarning("Freelancer with ID: {FreelancerId} not found", id);
                    return null;
                }

                var freelancerResult = _mapper.Map<FreelancerProfileResult>(profile);
               
                _logger.LogInformation("Successfully retrieved freelancer profile for ID: {FreelancerId}", id);
                return freelancerResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving freelancer profile for ID: {FreelancerId}", id);
                throw;
            }
        }
        #endregion



        public async Task<bool> AddOfferedService(AddOfferedServiceDto serviceDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return false;

            var spec = new CustomerWithFreelancerSpec(currentUser.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(spec);
            var freelancer = customer.Freelancer;
           
            _unitOfWork.CreateTransaction();

            var image = await _userHelpers.AddImage(serviceDto.Image, Consts.Consts.Freelancer);
            var offeredService = _mapper.Map<OfferedService>(serviceDto);
            offeredService.Image = image;
           


            try
            {
                if (freelancer != null)
                { 
                    offeredService.FreelancerId=freelancer.Id;
                     offeredService.Freelancer=freelancer;
                    _unitOfWork.OfferedService.Add(offeredService);
                    _unitOfWork.Save();
                }
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                await _userHelpers.DeleteImageAsync(image, Consts.Consts.Freelancer);
                return false;
            }
        }

        public async Task<bool> UpdateOfferedService(string id, AddOfferedServiceDto serviceDto)
        {
            var offeredService = _unitOfWork.OfferedService.GetById(id);
            var currentUser = await _userHelpers.GetCurrentUserAsync();

            if  (currentUser.Freelancer != null)
            {
                    _unitOfWork.CreateTransaction();
                try
                {
                    string newImagePath = await _userHelpers.AddImage(serviceDto.Image,Consts.Consts.Freelancer);
                    var oldImagePath=offeredService.Image;
                    offeredService = _mapper.Map(serviceDto, offeredService);
                    offeredService.Image = newImagePath;
                    _unitOfWork.OfferedService.Update(offeredService);
                    var saved = _unitOfWork.Save();
                    _unitOfWork.Commit();
                    if( saved > 0)
                        if(oldImagePath != null)
                            await _userHelpers.DeleteImageAsync(oldImagePath, Consts.Consts.Freelancer);
                    else
                        await _userHelpers.DeleteImageAsync(newImagePath, Consts.Consts.Freelancer);
                    return true;
                }
                catch
                {
                    _unitOfWork.Rollback();
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
            var user = await _userHelpers.GetCurrentUserAsync();
            var oldFreelancerBusiness = user.Freelancer;

            try
            {
                oldFreelancerBusiness = _mapper.Map(freelancerDto, oldFreelancerBusiness);
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
                try
                {
                    var user = await _userHelpers.GetCurrentUserAsync();
                    if (user == null)
                        return false;

                    var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "Freelancer");
                    if (!removeRoleResult.Succeeded)
                    {
                        return false;
                    }

                    var freelancer = user.Freelancer;
                    if (freelancer != null)
                    {
                        _unitOfWork.FreelancerBusiness.Remove(freelancer);
                    }
                    else
                    {
                        return false;
                    }

                    _unitOfWork.Save();
                    return true;
                }
                catch (Exception ex)
                {
                
                    return false;
                }
            }


        public async Task<List<OfferedServiceResult>> GetOfferedServicesAsync(string freelancerId, int pageIndex ,int pageSize)
        {
            var spec = new PaginatedOfferedServicesSpec(freelancerId, pageIndex, pageSize);

            var offeredServices = await _unitOfWork.OfferedService.GetAllWithSpecAsync(spec);

            var offeredServiceResults = _mapper.Map<List<OfferedServiceResult>>(offeredServices);

            return offeredServiceResults;
        }
           public async Task<List<FreelancerServiceRequistResult>> GetFreelancerRequests( int pageIndex ,int pageSize)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return null;

            var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(currentUser.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
            var paginatedFreelancerRequestsSpec = new PaginatedFreelancerRequestsSpec(customer.Freelancer.Id, pageIndex, pageSize);

            var request = await _unitOfWork.ServiceRequest.GetAllWithSpecAsync(paginatedFreelancerRequestsSpec);

            var requestResults = _mapper.Map<List<FreelancerServiceRequistResult>>(request);

            return requestResults;   
        }




        #region file handlling
        public async Task<bool> AddFreelancerPictureAsync(IFormFile file)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return false;

            var spec = new CustomerWithFreelancerSpec(currentUser.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(spec);
            var freelancer = customer.Freelancer;
            if (freelancer == null) return false;
            var picture = await _userHelpers.AddImage(file, Consts.Consts.Freelancer);
            if (picture != null)
                freelancer.Image = picture;
            _unitOfWork.FreelancerBusiness.Update(freelancer);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteFreelancerPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            var freelancer = user.Freelancer;
            if (freelancer == null) return false;
            var oldPicture = freelancer.Image;
            freelancer.Image = null;
            _unitOfWork.FreelancerBusiness.Update(freelancer);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteImageAsync(oldPicture, Consts.Consts.Freelancer);
            return false;
        }

        public async Task<bool> UpdateFreelancerPictureAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            var freelancer = user.Freelancer;
            if (freelancer == null) return false;
            var newPicture = await _userHelpers.AddImage(file, Consts.Consts.Freelancer);
            var oldPicture = freelancer.Image;
            freelancer.Image = newPicture;
            _unitOfWork.FreelancerBusiness.Update(freelancer);
            if (_unitOfWork.Save() > 0 && !oldPicture.IsNullOrEmpty())
            {
                return await _userHelpers.DeleteImageAsync(oldPicture, Consts.Consts.Freelancer);
            }
            await _userHelpers.DeleteImageAsync(newPicture, Consts.Consts.Freelancer);
            return false;
        }

        public async Task<string> GetFreelancerPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            var freelancer = user.Freelancer;
            if (freelancer == null)
                throw new Exception("freelancer not found");
            else if (freelancer.Image.IsNullOrEmpty())
                throw new Exception("freelancer dont have profile image");
            return freelancer.Image;
        }
        #endregion


    }
} 


