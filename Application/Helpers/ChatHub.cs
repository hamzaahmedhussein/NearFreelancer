    using Connect.Application.Services;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
namespace Connect.Application.Helpers
{
    public class ChatHub : Hub
    {
        private readonly IUserHelpers _userHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public ChatHub(IHttpContextAccessor httpContextAccessor,IUserHelpers userHelpers , IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _userHelper = userHelpers;
            _unitOfWork = unitOfWork;
        }

        public async Task SendPrivateMessage(string customerId, string freelancerId, string message,Sender sender)
        {
            var chatMessage = new ChatMessage
            {
               CustomerId=customerId,
               FreelancerId= freelancerId,
                Content = message,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.ChatRepository.AddMessage(chatMessage);
            await _unitOfWork.ChatRepository.SaveChangesAsync();
            if(sender==Sender.Freelancer)
            await Clients.User(customerId).SendAsync("ReceiveMessage", message);
            else
                await Clients.User(freelancerId).SendAsync("ReceiveMessage", message);

        }

        public async Task SendGroupMessage(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task<List<ChatMessage>> GetPreviousMessages(string customerId, string freelancerId)
        {
            var messages = await _unitOfWork.ChatRepository.GetMessagesBetweenUsers(customerId, freelancerId);

            return messages.OrderBy(m => m.Timestamp).ToList();  
        }



    }

}


