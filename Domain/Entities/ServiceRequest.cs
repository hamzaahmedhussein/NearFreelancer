using Connect.Core.Entities;

namespace Connect.Core.Models
{
    public class ServiceRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public RequisStatus? Status { get; set; }
        public decimal Price { get; set; }
        public string? FreelancerId { get; set; }
        public virtual Freelancer Freelancer { get; set; }
        public string? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
       

    }

    public enum RequisStatus
    {
        Pending,
        Accepted,
        Refused,
        Completed

    }
}
