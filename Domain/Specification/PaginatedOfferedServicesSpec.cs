using Connect.Core.Entities;
using Connect.Core.Specification;

namespace Connect.Application.Specifications
{
    public class PaginatedOfferedServicesSpec : Specification<OfferedService>
    {
        public PaginatedOfferedServicesSpec(string freelancerId, int pageIndex, int pageSize)
            : base(os => os.FreelancerId == freelancerId)
        {
            ApplyPaging(pageSize, (pageIndex - 1) * pageSize);
        }
    }
}
