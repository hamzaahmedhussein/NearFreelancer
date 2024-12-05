using Connect.Core.Entities;
using Connect.Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Infrastructure.Repsitory_UOW
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetMessagesBetweenUsers(string customerId, string freelancerId)
        {
            return await _context.ChatMessages
                .Where(m => (m.CustomerId == customerId && m.FreelancerId == freelancerId) ||
                            (m.CustomerId == freelancerId && m.FreelancerId == customerId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
        public async Task AddMessage(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
