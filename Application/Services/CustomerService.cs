using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Application.MailSettings;
using Connect.Core.Interfaces;
using Connect.Application.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Connect.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Connect.Application.Specifications;

namespace Connect.Application.Services
{
    public class CustomerService : ICustomerService
    {
        #region Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<CustomerService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly IMailingService _mailingService;
        private readonly SignInManager<Customer> _signInManager;

        public CustomerService(
            IUnitOfWork unitOfWork,
            UserManager<Customer> userManager,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ILogger<CustomerService> logger,
            IUserHelpers userHelpers,
            IMailingService mailingService,
            SignInManager<Customer> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _mailingService = mailingService;
            _signInManager = signInManager;
        }
        #endregion

        #region Registration

        public async Task<IdentityResult> Register(RegisterUserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            IdentityResult createResult = await CreateUserAsync(userDto);
            if (!createResult.Succeeded)
                return createResult;

            IdentityResult roleResult = await _userHelpers.AddUserToRoleAsync(userDto.Email, "Customer");
            if (!roleResult.Succeeded)
                return roleResult;

            try
            {
                await SendConfirmationEmailAsync(userDto.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to user with email {Email}", userDto.Email);
                return IdentityResult.Failed(new IdentityError { Description = "Failed to send confirmation email. Please try again later." });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterUserDto userDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });
            }

            var customer = _mapper.Map<Customer>(userDto);
            return await _userManager.CreateAsync(customer, userDto.Password);
        }


        public async Task SendConfirmationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("User not found.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/Account/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";

            var message = new MailMessage(new[] { user.Email }, "Confirmation Email", confirmationLink);

             _mailingService.SendMail(message);
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        #endregion

        #region ForgetPassword
        public async Task<bool> ForgetPassword(string email)
        {
            var customer = await _userManager.FindByEmailAsync(email);
            if (customer == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(customer);
            var resetLink = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/Account/reset-password?email={Uri.EscapeDataString(customer.Email)}&token={Uri.EscapeDataString(token)}";
            var message = new MailMessage(new string[] { customer.Email }, "reset email link", resetLink);
            _mailingService.SendMail(message);
            return true;
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var customer = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (customer == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var result = await _userManager.ResetPasswordAsync(customer, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            return result;
        }
        #endregion

        #region ChangePassword
        public async Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            return result;
        }
        #endregion

        #region Login
        public async Task<LoginResult> Login(LoginUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return CreateLoginResult(false, null, LoginErrorType.UserNotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return CreateLoginResult(false, null, LoginErrorType.InvalidPassword);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return CreateLoginResult(false, null, LoginErrorType.EmailNotConfirmed);
            }

            var claims = _userHelpers.CreateClaimsForUser(user);

            return await _userHelpers.GenerateJwtTokenAsync(claims);
        }
        private LoginResult CreateLoginResult(bool success, string token, LoginErrorType errorType)
        {
            return new LoginResult
            {
                Success = success,
                Token = token,
                Expiration = default,
                ErrorType = errorType
            };
        }

        #endregion

        #region Logout
        public async Task<LogoutResult> LogoutAsync()
        {
            try
            {
                var currentUser = await _userHelpers.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return new LogoutResult
                    {
                        Success = false,
                        Message = "User Not Found"
                    };
                }

                await _signInManager.SignOutAsync();
                return new LogoutResult
                {
                    Success = true,
                    Message = "User successfully logged out."
                };
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return new LogoutResult
                {
                    Success = false,
                    Message = "An error occurred while logging out."
                };
            }
        }
        #endregion

        #region GetCurrentProfile
        public async Task<CustomerProfileResult> GetCurrentProfileAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();

            if (currentUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }


             var user=_mapper.Map<CustomerProfileResult>(currentUser);
            //var requests = await _unitOfWork.ServiceRequest.FindAsync(r => r.CustomerId == currentUser.Id);
            //var requestsResult = _mapper.Map< IEnumerable < CustomerServiceRequestResult >> (requests);
            //user.Requests = requestsResult.ToList();
            return user;
        }
        #endregion

        #region GetCustomerById
        public async Task<CustomerProfileResult> GetCustomerById(string id)
        {
            try
            {
                _logger.LogInformation("Getting customer profile for ID: {customerId} with requests", id);


                var profile = _unitOfWork.Customer.GetById(id);
                if (profile == null)
                {
                    _logger.LogWarning("Customer with ID: {CustomerId} not found", id);
                    return null;
                }

                var result = _mapper.Map<CustomerProfileResult>(profile);
                //var requests = await _unitOfWork.ServiceRequest.FindAsync(r => r.CustomerId == id);
                //var requestsResult = requests.Select(request => _mapper.Map<CustomerServiceRequestResult>(request));
                //result.Requests = requestsResult.ToList();
                _logger.LogInformation("Successfully retrieved customer profile for ID: {customerId}", id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving freelancer profile for ID: {CustomerrId}", id);
                throw;
            }
        }
        #endregion

        #region SendServiceRequest
        public async Task<bool> SendServiceRequist(string Id, SendServiceRequestDto request)
        {
            var customer = await _userHelpers.GetCurrentUserAsync();
            if (customer == null)
                return false;

            var freelancer = _unitOfWork.FreelancerBusiness.GetById(Id);
            if (freelancer == null)
                return false;

            var serviceRequest = _mapper.Map<ServiceRequest>(request);
            serviceRequest.FreelancerId = Id;
            serviceRequest.Freelancer = freelancer;
            serviceRequest.Customer = customer;
            _unitOfWork.ServiceRequest.Add(serviceRequest);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region GetMyRequests
        public async Task<IEnumerable<CustomerServiceRequestResult>> GetMyRequests(int pageIndex, int pageSize)
        {
            var customer = await _userHelpers.GetCurrentUserAsync();
            if (customer == null)
                throw new Exception("User not found");

            var paginatedCustomerRequestsSpec = new PaginatedCustomerRequestsSpec(customer.Id, pageIndex, pageSize);

            var request = await _unitOfWork.ServiceRequest.GetAllWithSpecAsync(paginatedCustomerRequestsSpec);

            var requestResults = _mapper.Map<List<CustomerServiceRequestResult>>(request);

            return requestResults;
        }
        #endregion

        #region DeleteAccount
        public async Task<bool> DeleteAccountAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        #endregion

        #region UpdateCustomerInfo
        public async Task<bool> UpdateCustomerInfo(UpdateCustomerInfoDto updateDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                return false;

            user = _mapper.Map(updateDto, user);

            _unitOfWork.Customer.Update(user);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region DeletePendingRequest
        public async Task<bool> DeletePendingRequestAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Invalid request ID");

            var request = _unitOfWork.ServiceRequest.GetById(id);

            if (request == null || request.Customer != await _userHelpers.GetCurrentUserAsync())
                return false;

            if (request.Status != RequisStatus.Pending)
                return false;

            try
            {
                _unitOfWork.ServiceRequest.Remove(request);

                _unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region file handlling
        public async Task<bool> AddCustomerPictureAsync(IFormFile file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var picture = await _userHelpers.AddImage(file, Consts.Consts.Customer);
            if (picture != null)
                user.Image = picture;
            _unitOfWork.Customer.Update(user);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteCustomerPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            if (user.Image == "/Images/default/avatar") return true;
            var oldPicture = user.Image;
            user.Image = "/Images/default/avatar";
            _unitOfWork.Customer.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteImageAsync(oldPicture, Consts.Consts.Customer);
            return false;
        }

        public async Task<bool> UpdateCustomerPictureAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newPicture = await _userHelpers.AddImage(file, Consts.Consts.Customer);
            var oldPicture = user.Image;
            user.Image = newPicture;
            _unitOfWork.Customer.Update(user);
            if (_unitOfWork.Save() > 0 && !oldPicture.IsNullOrEmpty())
            {
                if(oldPicture != "/Images/default/avatar")
                    return await _userHelpers.DeleteImageAsync(oldPicture, Consts.Consts.Customer);
                return true;
            }
            await _userHelpers.DeleteImageAsync(newPicture, Consts.Consts.Customer);
            return false;
        }

        public async Task<string> GetCustomerPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("customer not found");
            else if (user.Image.IsNullOrEmpty())
                throw new Exception("customer dont have profile image");
            return user.Image;
        }
        #endregion

    }
}
