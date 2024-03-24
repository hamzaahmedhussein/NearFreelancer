using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Connect.Application.Helpers;
using Connect.Application.Settings;
namespace Connect.Application.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Customer> _userManager;
        private readonly IConfiguration _config;
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
            IMailingService mailingService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _config = config;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _mailingService = mailingService;
        }
        public async Task<IdentityResult> Register(RegisterUserDto userDto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(userDto.Email);

            if (existingUserByEmail != null)
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });

            var existingUserByUsername = await _userManager.FindByNameAsync(userDto.UserName);

            if (existingUserByUsername != null)
                return IdentityResult.Failed(new IdentityError { Description = "User with this username already exists." });
            var customer = _mapper.Map<Customer>(userDto);
            IdentityResult result=await _userManager.CreateAsync(customer,userDto.Password);
            return result;  
        }

        public async Task<LoginResult> Login(LoginUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return new LoginResult { Success = false, Token = null, Expiration = default };
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




        public async Task<bool> SendPasswordResetEmail(string userEmail)
        {
            try
            {
                var token = "Hi"; 
                var resetLink = $"https://yourdomain.com/reset-password?token={token}&email={Uri.EscapeDataString(userEmail)}";

                var subject = "Password Reset Request";
                var body = $"<p>You have requested to reset your password. Please click on the link below to reset your password:</p><a href=\"{resetLink}\">Reset Password</a><p>If you did not request a password reset, please ignore this email.</p>";

                await _mailingService.SendMailAsync(userEmail, subject, body);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> ResetPassword(ResetPasswordDto resetDto)
        {
            var user = await _userManager.FindByEmailAsync(resetDto.Email);
            if (user == null)
                return false;

            var result = await _userManager.ResetPasswordAsync(user, resetDto.Token, resetDto.NewPassword);
            return result.Succeeded;
        }

  
        public IEnumerable<HomePageFilterDto> GetFilteredProviders(HomePageFilterDto filterDto)
        {
            if (filterDto == null)
            {
                return Enumerable.Empty<HomePageFilterDto>();
            }
            else if (filterDto.ProviderType == ProviderType.ReservationProvider)
            {
                var providersQuery = _unitOfWork.ReservationBusiness.GetAll();


                if (!string.IsNullOrWhiteSpace(filterDto.Name))
                {
                    providersQuery = providersQuery.Where(p => p.Name.Contains(filterDto.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Location))
                {
                    providersQuery = providersQuery.Where(p => p.Location.Contains(filterDto.Location, StringComparison.OrdinalIgnoreCase));
                }

                if (filterDto.Capability != null && filterDto.Capability.Any())
                {
                    providersQuery = providersQuery.Where(p => p.FeatureList.Any(capability => filterDto.Capability.Contains(capability)));
                }

                providersQuery = providersQuery.Where(p => p.AvailableFrom <= filterDto.AvailableTo && p.AvailableTo >= filterDto.AvailableFrom);

                var providerDtos = providersQuery.Select(p => new HomePageFilterDto
                {
                }).ToList();

                return providerDtos;
            }
            else
            {
                var providersQuery = _unitOfWork.FreelancerBusiness.GetAll();


                if (!string.IsNullOrWhiteSpace(filterDto.Name))
                {
                    providersQuery = providersQuery.Where(p => p.Name.Contains(filterDto.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Location))
                {
                    providersQuery = providersQuery.Where(p => p.Location.Contains(filterDto.Location, StringComparison.OrdinalIgnoreCase));
                }

                if (filterDto.Capability != null && filterDto.Capability.Any())
                {
                    providersQuery = providersQuery.Where(p => p.Skills.Any(capability => filterDto.Capability.Contains(capability)));
                }

                providersQuery = providersQuery.Where(p => p.AvailableFrom <= filterDto.AvailableTo && p.AvailableTo >= filterDto.AvailableFrom);

                var providerDtos = providersQuery.Select(p => new HomePageFilterDto
                {
                }).ToList();

                return providerDtos;
            }
        }
    }
}
    

    

