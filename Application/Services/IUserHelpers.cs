using Connect.Application.DTOs;
using Connect.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    public interface IUserHelpers
    {
        Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims);
        Task<Customer> GetCurrentUserAsync();
        Task<string> AddFreelancerServiceImage(IFormFile file);
        Task<string> AddFreelancerProfileImage(IFormFile? file);
    }
}
