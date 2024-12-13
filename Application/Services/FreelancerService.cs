using AutoMapper;
using Connect.Application.DTOs;
using Connect.Application.Specifications;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Connect.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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


                var profile = _unitOfWork.FreelancerBusiness.GetById(id);
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
                    offeredService.FreelancerId = freelancer.Id;
                    offeredService.Freelancer = freelancer;
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

            if (currentUser.Freelancer != null)
            {
                _unitOfWork.CreateTransaction();
                try
                {
                    string newImagePath = await _userHelpers.AddImage(serviceDto.Image, Consts.Consts.Freelancer);
                    var oldImagePath = offeredService.Image;
                    offeredService = _mapper.Map(serviceDto, offeredService);
                    offeredService.Image = newImagePath;
                    _unitOfWork.OfferedService.Update(offeredService);
                    var saved = _unitOfWork.Save();
                    _unitOfWork.Commit();
                    if (saved > 0)
                        if (oldImagePath != null)
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

        public async Task<PaginatedResponse<FreelancerResult>> FilterFreelancers(string search, int pageIndex, int pageSize)
        {
            var spec = new PaginatedFilteredFreelancers(search, pageIndex, pageSize);

            var results = await _unitOfWork.FreelancerBusiness.GetAllWithSpecAsync(spec);

            var totalCount = await _unitOfWork.FreelancerBusiness.CountAsync(spec);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var mappedResults = _mapper.Map<IReadOnlyList<FreelancerResult>>(results);

            return new PaginatedResponse<FreelancerResult>
            {
                Data = mappedResults,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages
            };
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
            if (user == null)
                return false;

            var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(user.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
            var oldFreelancerBusiness = customer.Freelancer;


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
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                return false;

            try
            {
                var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(user.Id);
                var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
                var freelancer = customer.Freelancer;

                if (freelancer != null)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, "Freelancer");

                    _unitOfWork.FreelancerBusiness.Remove(freelancer);

                    _unitOfWork.Save();

                    return result.Succeeded;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting freelancer business: {ex}");

                // Rethrow the exception to propagate it to the caller
                throw;
            }
        }



        public async Task<PaginatedResponse<OfferedServiceResult>> GetOfferedServicesAsync(string freelancerId, int pageIndex, int pageSize)
        {
            var spec = new PaginatedOfferedServicesSpec(freelancerId, pageIndex, pageSize);

            var offeredServices = await _unitOfWork.OfferedService.GetAllWithSpecAsync(spec);

            var offeredServiceResults = _mapper.Map<List<OfferedServiceResult>>(offeredServices);

            var totalCount = await _unitOfWork.OfferedService.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);


            return new PaginatedResponse<OfferedServiceResult>
            {
                Data = offeredServiceResults,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
        public async Task<PaginatedResponse<FreelancerServiceRequistResult>> GetFreelancerRequests(int pageIndex, int pageSize)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return null;

            var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(currentUser.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
            var paginatedFreelancerRequestsSpec = new PaginatedFreelancerRequestsSpec(customer.Freelancer.Id, pageIndex, pageSize);

            var request = await _unitOfWork.ServiceRequest.GetAllWithSpecAsync(paginatedFreelancerRequestsSpec);

            var requestResults = _mapper.Map<List<FreelancerServiceRequistResult>>(request);

            var totalCount = await _unitOfWork.ServiceRequest.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);


            return new PaginatedResponse<FreelancerServiceRequistResult>
            {
                Data = requestResults,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages
            };
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
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return false;

            var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(currentUser.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
            var freelancer = customer.Freelancer;

            if (freelancer == null) return false;
            if (freelancer.Image == "/Images/default/avatar") return true;

            var oldPicture = freelancer.Image;
            freelancer.Image = "/Images/default/avatar";
            _unitOfWork.FreelancerBusiness.Update(freelancer);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteImageAsync(oldPicture, Consts.Consts.Freelancer);
            return false;
        }

        public async Task<bool> UpdateFreelancerPictureAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();

            var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(user.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
            var freelancer = customer.Freelancer;

            if (freelancer == null) return false;
            var newPicture = await _userHelpers.AddImage(file, Consts.Consts.Freelancer);
            var oldPicture = freelancer.Image;
            freelancer.Image = newPicture;
            _unitOfWork.FreelancerBusiness.Update(freelancer);
            if (_unitOfWork.Save() > 0 && !oldPicture.IsNullOrEmpty())
            {
                if (oldPicture != "/Images/default/avatar")
                    return await _userHelpers.DeleteImageAsync(oldPicture, Consts.Consts.Freelancer);
                return true;
            }
            await _userHelpers.DeleteImageAsync(newPicture, Consts.Consts.Freelancer);
            return false;
        }

        public async Task<string> GetFreelancerPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            var customerWithFreelancerSpec = new CustomerWithFreelancerSpec(user.Id);
            var customer = await _unitOfWork.Customer.GetByIdWithSpecAsync(customerWithFreelancerSpec);
            var freelancer = customer.Freelancer;
            if (freelancer == null)
                throw new Exception("freelancer not found");
            else if (freelancer.Image.IsNullOrEmpty())
                throw new Exception("freelancer dont have profile image");
            return freelancer.Image;
        }

        #endregion



    }
}


