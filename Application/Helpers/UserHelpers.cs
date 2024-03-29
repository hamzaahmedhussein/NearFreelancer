using Connect.Application.DTOs;
using Connect.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Connect.Application.Helpers
{
    public  class UserHelpers : IUserHelpers
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserHelpers(IConfiguration config, UserManager<Customer> userManager,IHttpContextAccessor contextAccessor)
        {

            _config = config;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public  async Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResult {
                Success=true,
                Token= tokenString,
                Expiration= token.ValidTo
            };
        }

        public async Task<Customer> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }
        public async Task ChangeUserTypeAsync(int type, Customer user)
        {
            if (type == 0)
                await _userManager.AddToRoleAsync(user, "Customer");
            else if (type == 1)
                await _userManager.AddToRoleAsync(user, "Freelancer");
            else  if (type == 2)
                await _userManager.AddToRoleAsync(user, "ReservationProvider");
        }
    }
}
