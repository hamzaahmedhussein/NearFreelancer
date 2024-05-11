using Connect.Core.Entities;
using Connect.Core.Models;
using Connect.Core.Specification;

namespace Connect.Application.Specifications
{
    public class PaginatedFreelancerRequestsSpec : Specification<ServiceRequest>
    {
        public PaginatedFreelancerRequestsSpec(string freelancerId, int pageSize, int pageIndex)
            : base(os => os.FreelancerId == freelancerId)
        {
            ApplyPaging(pageIndex, pageIndex * pageSize);
        }
    }
}
