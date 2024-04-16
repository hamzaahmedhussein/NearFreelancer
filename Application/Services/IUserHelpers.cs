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
        Task<string> AddImage(IFormFile file,string folderName);
        Task<Message> SendMessage(string content, string recipientId);
        Task<List<Message>> GetConversation(string userId, string recipientId);
        Task DeleteImageAsync(string fileName, string folderName);
        Task<string> UpdateImageAsync(IFormFile? file, string fileName, string folderName);
    }
}
