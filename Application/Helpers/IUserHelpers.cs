using Connect.Application.DTOs;
using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Helpers
{
    public interface IUserHelpers 
    {
        Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims);
        Task<Customer> GetCurrentUserAsync();
        public  Task ChangeUserTypeAsync(int type, Customer user);

    }
}
