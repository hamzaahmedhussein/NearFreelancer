using Connect.Core.Models;
using Connect.Core.Specification;

namespace Connect.Application.Specifications
{
    public class PaginatedCustomerRequestsSpec : Specification<ServiceRequest>
    {
        public PaginatedCustomerRequestsSpec(string customerId, int pageIndex, int pageSize)
            : base(os => os.CustomerId == customerId)
        {
            ApplyPaging(pageSize, (pageIndex - 1) * pageSize);
        }
    }
}
