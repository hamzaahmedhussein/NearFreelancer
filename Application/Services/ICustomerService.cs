using Connect.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Connect.Application.Services
{
    public interface ICustomerService
    { 
        Task<IdentityResult> Register(RegisterUserDto userDto);
        Task<LoginResult> Login(LoginUserDto userDto);
        Task<bool> ConfirmEmail(string email, string token);
        Task<bool> ForgetPassword(string email);
        Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<CurrentProfileResult> GetCurrentProfileAsync();
        IEnumerable<HomePageFilterDto> GetFilteredProviders(HomePageFilterDto filterDto);



    }
}
