using Connect.Application.DTOs;
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
        Task<string> LogoutAsync();
        Task<bool> DeleteAccountAsync();
        Task<List<string>> GetUserRolesAsync();
        Task<bool> UpdateCustomerInfo(UpdateCustomerInfoDto updateDto);
        Task<bool> ConfirmEmail(string email, string token);
        Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordDto);
        public Task<string> ForgotPasswordAsync(string Email);
        public Task<string> VerifyOTPAsync(VerifyOTPDto Model);
        public Task<string> ResetPasswordAsync(ResetPasswordDto Model); Task<CustomerProfileResult> GetCurrentProfileAsync();
        Task<CustomerProfileResult> GetCustomerById(string id);
        Task<PaginatedResponse<CustomerServiceRequestResult>> GetMyRequests(int pageIndex, int pageSize);
        Task<bool> SendServiceRequist(SendServiceRequestDto request);
        Task<bool> DeletePendingRequestAsync(string id);
        void SetRefreshTokenInCookie(string token, DateTime expires);
        Task<RefreshTokenResult> RefreshTokenAsync();
        Task<bool> RevokeTokenAsync(string Token);

        // IEnumerable<HomePageFilterDto> GetFilteredProviders(HomePageFilterDto filterDto);

        Task<bool> AddCustomerPictureAsync(IFormFile file);
        Task<bool> DeleteCustomerPictureAsync();
        Task<bool> UpdateCustomerPictureAsync(IFormFile? file);
        Task<string> GetCustomerPictureAsync();
    }
}
