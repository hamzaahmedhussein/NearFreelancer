using Connect.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    public interface ICustomerService
    { 
        Task<IdentityResult> Register(RegisterUserDto userDto);
        Task<LoginResult> Login(LoginUserDto userDto);
        Task<CurrentProfileResult> GetCurrentProfileAsync();
        Task<bool> SendPasswordResetEmail(string email);
        Task<bool> ResetPassword(ResetPasswordDto resetDto);        
        IEnumerable<HomePageFilterDto> GetFilteredProviders(HomePageFilterDto filterDto);



    }
}
