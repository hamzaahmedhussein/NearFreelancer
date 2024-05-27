using Connect.Core.Models;

namespace Connect.Application.DTOs
{
    public class CustomerServiceRequestResult
    {
        public string Name { get; set; }
        public RequisStatus Status { get; set; }
        public  string FreelancerId { get; set; }
        public  string FreelancerName { get; set; }

    }
}
