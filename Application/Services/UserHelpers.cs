#region imports
using Connect.Application.DTOs;
using Connect.Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#endregion


namespace Connect.Application.Services
{
    public class UserHelpers : IUserHelpers
    {
        #region Constructor
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IUserHelpers> _logger;

        public UserHelpers(IConfiguration config, UserManager<Customer> userManager, IHttpContextAccessor contextAccessor,
            IWebHostEnvironment webHostEnvironment, ApplicationDbContext context , ILogger<IUserHelpers> logger)
        {
            _config = config;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _logger = logger;
        }
        #endregion

        public async Task<IdentityResult> AddUserToRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError("User with email {Email} not found for role assignment", email);
                return IdentityResult.Failed(new IdentityError { Description = $"User with email {email} not found." });
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        #region JWT
        public List<Claim> CreateClaimsForUser(Customer user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
        {
            var jwtConfig = _config.GetSection("JWT");
            var secretKey = jwtConfig["Secret"];
            var validIssuer = jwtConfig["ValidIssuer"];
            var validAudience = jwtConfig["ValidAudience"];

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(validIssuer) || string.IsNullOrEmpty(validAudience))
            {
                throw new InvalidOperationException("JWT configuration is not set properly.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.UtcNow.AddDays(1); // Consider using a configuration value for expiration time
            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResult
            {
                Success = true,
                Token = tokenString,
                Expiration = token.ValidTo
            };
        }
        #endregion

        #region User Management
        public async Task<Customer> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }
        #endregion

        #region Image Management
        public async Task<string> AddImage(IFormFile? file, string profileType)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;
            string profileFolderPath = Path.Combine(rootPath, "Images", userName, profileType);

            if (!Directory.Exists(profileFolderPath))
            {
                Directory.CreateDirectory(profileFolderPath);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(profileFolderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filePath;
        }

        
        public async Task<bool> DeleteImageAsync(string imagePath, string profileType)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Image path is null or empty.", nameof(imagePath));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;

            if (!imagePath.StartsWith($"/Images/{userName}/{profileType}/"))
            {
                throw new ArgumentException("Invalid image path.", nameof(imagePath));
            }

            string filePath = Path.Combine(rootPath, imagePath.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
        } 

        public async Task<string> UpdateImageAsync(IFormFile? file, string oldImagePath, string folderName)
        {
            await DeleteImageAsync(oldImagePath, folderName);
            string newImagePath = await AddImage(file, folderName);
            return newImagePath;
        }
        #endregion

        //#region Messaging
        //public async Task<Message> SendMessage(string content, string recipientId)
        //{
        //    var user = await GetCurrentUserAsync();
        //    var message = new Message
        //    {
        //        Content = content,
        //        SentAt = DateTime.UtcNow,
        //        SenderId = user.Id,
        //        RecipientId = recipientId,
        //    };

        //    _context.Messages.Add(message);
        //    await _context.SaveChangesAsync();

        //    return message;
        //}

        //public async Task<List<Message>> GetConversation(string userId, string recipientId)
        //{
        //    return await _context.Messages
        //      .Where(m => (m.SenderId == userId && m.RecipientId == recipientId) || (m.SenderId == recipientId && m.RecipientId == userId))
        //      .OrderBy(m => m.SentAt)
        //      .ToListAsync();
        //}
        //#endregion
    }
}
