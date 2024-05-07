using Connect.Core.Models;

namespace Connect.Application.DTOs
{
    public class ServiceRequestResult
    {
        public string? Name { get; set; }
        public RequisStatus Status { get; set; }
        public  RequestedFreelancerDto Freelancer { get; set; }

    }
}
