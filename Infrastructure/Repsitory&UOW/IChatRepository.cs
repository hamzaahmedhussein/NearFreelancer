using Connect.Core.Entities;

namespace Connect.Infrastructure.Repsitory_UOW
{
    public interface IChatRepository
    {
        Task<List<ChatMessage>> GetMessagesBetweenUsers(string customerId, string freelancerId);
        Task AddMessage(ChatMessage message);
        Task SaveChangesAsync();
    }


}
