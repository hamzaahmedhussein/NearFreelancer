using Connect.Application.DTOs;
using Connect.Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Connect.Application.Services
{
    public class UserHelpers : IUserHelpers
    {
        IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;


        public UserHelpers(IConfiguration config, UserManager<Customer> userManager, IHttpContextAccessor contextAccessor
            , IWebHostEnvironment webHostEnvironment,ApplicationDbContext context)
        {
            _config = config;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        public async Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
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

            return new LoginResult
            {
                Success = true,
                Token = tokenString,
                Expiration = token.ValidTo
            };
        }

        public async Task<Customer> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);

        }

    public async Task<string> AddImage(IFormFile? file,string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;
            string userFolderPath = Path.Combine(rootPath, "Images", userName);
            string profileFolderPath = Path.Combine(userFolderPath, folderName);

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

            return $"/Images/{userName}/{folderName}/{fileName}";
        }

        //public async Task<string> AddFreelancerImage(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        throw new ArgumentException("File is null or empty.", nameof(file));
        //    }

        //    string rootPath = _webHostEnvironment.WebRootPath;
        //    var user = await GetCurrentUserAsync();
        //    string userName = user.UserName;
        //    string userFolderPath = Path.Combine(rootPath, "Images", userName);
        //    string FrelanceFolderPath = Path.Combine(userFolderPath, "Freelancer");

        //    if (!Directory.Exists(FrelanceFolderPath))
        //    {
        //        Directory.CreateDirectory(FrelanceFolderPath);
        //    }

        //    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    string filePath = Path.Combine(FrelanceFolderPath, fileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }

        //    return $"/Images/{userName}/Freelancer/{fileName}";
        //}


        public async Task DeleteImageAsync(string imagePath, string folderName)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Image path is null or empty.", nameof(imagePath));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;

            
            if (!imagePath.StartsWith($"/Images/{userName}/{folderName}/"))
            {
                throw new ArgumentException("Invalid image path.", nameof(imagePath));
            }

            string filePath = Path.Combine(rootPath, imagePath.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
        }



        public async Task<string> UpdateImageAsync(IFormFile? file, string oldImagePath, string folderName)
        {
            if (string.IsNullOrEmpty(oldImagePath))
            {
                throw new ArgumentException("Image path is null or empty.", nameof(oldImagePath));
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            var user = await GetCurrentUserAsync();
            string userName = user.UserName;

            if (!oldImagePath.StartsWith($"/Images/{userName}/{folderName}/"))
            {
                throw new ArgumentException("Invalid image path.", nameof(oldImagePath));
            }

            await DeleteImageAsync(oldImagePath, folderName);

            string newImagePath = await AddImage(file, folderName);

             return newImagePath;
        }





        public async Task<Message> SendMessage(string content, string recipientId)
        {
            var user = await GetCurrentUserAsync();
                var message = new Message
            {
                Content = content,
                SentAt = DateTime.UtcNow,
                SenderId = user.Id,
                RecipientId = recipientId,
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<Message>> GetConversation(string userId, string recipientId)
        {
            return await _context.Messages
              .Where(m => (m.SenderId == userId && m.RecipientId == recipientId) || (m.SenderId == recipientId && m.RecipientId == userId))
              .OrderBy(m => m.SentAt)
              .ToListAsync();
        }
    }
}




