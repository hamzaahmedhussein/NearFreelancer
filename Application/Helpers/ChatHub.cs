using Connect.Application.Services;
using Connect.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
namespace Connect.Application.Helpers
{
    public class ChatHub : Hub
    {
        private readonly IUserHelpers _userHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IHttpContextAccessor httpContextAccessor,IUserHelpers userHelpers)
        {
            _httpContextAccessor = httpContextAccessor;
            _userHelper = userHelpers;
        }

        //public async Task SendMessage(string message, string recipientId)
        //{
        //    var sentMessage = await _userHelper.SendMessage(message, recipientId);
        //    await Clients.Client(recipientId).SendAsync("ReceiveMessage", sentMessage);
        //}

        //public async Task<List<
        //    Message>> GetConversation(string recipientId)
        //{
        //    var user = await _userHelper.GetCurrentUserAsync(); 
        //    return await _userHelper.GetConversation(user.Id, recipientId);
        //}
    }

}


