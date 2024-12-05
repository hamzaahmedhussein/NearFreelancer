using Connect.Core.Models;

namespace Connect.Core.Entities
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public Sender Sender { get; set; }
    }
    public enum Sender
    {
        Customer,
        Freelancer
    }

}