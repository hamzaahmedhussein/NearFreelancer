using Connect.Core.Models;
using Connect.Core.Specification;

namespace Connect.Application.Specifications
{
    public class PaginatedFreelancerRequestsSpec : Specification<ServiceRequest>
    {
        public PaginatedFreelancerRequestsSpec(string freelancerId, int pageIndex, int pageSize)
            : base(os => os.FreelancerId == freelancerId)
        {
            ApplyPaging(pageSize, (pageIndex - 1) * pageSize);
        }
    }
}
