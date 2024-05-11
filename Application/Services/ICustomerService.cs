using Connect.Application.DTOs;
using Connect.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Connect.Application.Services
{
    public interface ICustomerService
    {
       // Task<bool> DeleteCustomerAsync();
        Task<IdentityResult> Register(RegisterUserDto userDto); 
        Task<IdentityResult> CreateUserAsync(RegisterUserDto userDto);
        Task<LoginResult> Login(LoginUserDto userDto);
        Task<LogoutResult> LogoutAsync();
        Task<bool> DeleteAccountAsync();
        Task<bool> UpdateCustomerInfo(UpdateCustomerInfoDto updateDto);
        Task<bool> ConfirmEmail(string email, string token);
        Task<bool> ForgetPassword(string email);
        Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<CustomerProfileResult> GetCurrentProfileAsync();
        Task<CustomerProfileResult> GetCustomerById(string id);
        Task<IEnumerable<CustomerServiceRequestResult>> GetMyRequests();
        Task<bool> SendServiceRequist(string Id, SendServiceRequestDto request);
        Task<bool> DeletePendingRequestAsync(string id);
       
        // IEnumerable<HomePageFilterDto> GetFilteredProviders(HomePageFilterDto filterDto);

        Task<bool> AddCustomerPictureAsync(IFormFile file);
        Task<bool> DeleteCustomerPictureAsync();
        Task<bool> UpdateCustomerPictureAsync(IFormFile? file);
        Task<string> GetCustomerPictureAsync();
    }
}
