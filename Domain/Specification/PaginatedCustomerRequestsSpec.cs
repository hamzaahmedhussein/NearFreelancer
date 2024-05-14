using Connect.Core.Entities;
using Connect.Core.Models;
using Connect.Core.Specification;

namespace Connect.Application.Specifications
{
    public class PaginatedCustomerRequestsSpec : Specification<ServiceRequest>
    {
        public PaginatedCustomerRequestsSpec(string customerId, int pageSize, int pageIndex)
            : base(os => os.CustomerId == customerId)
        {
            ApplyPaging(pageIndex, pageIndex * pageSize);
        }
    }
}
