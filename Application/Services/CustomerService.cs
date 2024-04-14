using Connect.Application.DTOs;
using Connect.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Connect.Application.Settings;
using Connect.Application.MailSettings;
using Connect.Core.Models;
using Connect.Core.Interfaces;
namespace Connect.Application.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger <CustomerService> _logger ;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly IMailingService _mailingService;


        public CustomerService(IUnitOfWork unitOfWork ,
            UserManager<Customer> userManager,
            IConfiguration config,IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ILogger<CustomerService> logger ,
            IUserHelpers userHelpers,
            IMailingService mailingService
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _mailingService = mailingService;


        }

        #region Registeration 
        public async Task<IdentityResult> Register(RegisterUserDto userDto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(userDto.Email);

            if (existingUserByEmail != null)
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });

              var customer = _mapper.Map<Customer>(userDto);
            IdentityResult result=await _userManager.CreateAsync(customer,userDto.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(customer, "Customer");



            var token= await _userManager.GenerateEmailConfirmationTokenAsync(customer);
            var confirmationLink = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/Account/confirm-email?email={Uri.EscapeDataString(customer.Email)}&token={Uri.EscapeDataString(token)}";
            var message = new MailMessage(new string[] { customer.Email },"Confirmation email link", confirmationLink);
            _mailingService.SendMail(message);
            return result;  
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
        public async Task<IdentityResult> ChangePassword( ChangePasswordDto changePasswordDto)
        {
            var user =  await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            return result;
        }
        #endregion



        public async Task<LoginResult> Login(LoginUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Token = null,
                    Expiration = default,
                    ErrorType = LoginErrorType.UserNotFound
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return new LoginResult
                {
                    Success = false,
                    Token = null,
                    Expiration = default,
                    ErrorType = LoginErrorType.InvalidPassword
                };
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return await _userHelpers.GenerateJwtTokenAsync(claims);
        }


        public async Task<CurrentProfileResult> GetCurrentProfileAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<CurrentProfileResult>(user);

        }

        public async Task<bool> SendServiceRequist(string Id, SendServiceRequestDto request)
        {
           var customer= await _userHelpers.GetCurrentUserAsync();
            if (customer == null) 
                return false;

            var freelancer=_unitOfWork.FreelancerBusiness.GetById(Id);
            if (freelancer == null)
                return false;


            var serviceRequist = _mapper.Map<ServiceRequest>(request);
            serviceRequist.Freelancer = freelancer;
            serviceRequist.Customer=customer;

            return true;

        }

        public async Task<IEnumerable<GetCustomerRequestsDto>> GetMyRequests()
        {
            var customer = await _userHelpers.GetCurrentUserAsync();
            if (customer == null)
                throw new Exception("User not found");

            var requests = await _unitOfWork.ServiceRequest.FindAsync(c=>c.CustomerId==customer.Id);

            if (requests == null)
                throw new Exception("No requests");


            return _mapper.Map<IEnumerable<GetCustomerRequestsDto>>(requests);
        }



    }
}
    

    

