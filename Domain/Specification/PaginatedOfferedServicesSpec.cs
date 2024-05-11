using Connect.Core.Entities;
using Connect.Core.Specification;

namespace Connect.Application.Specifications
{
    public class PaginatedOfferedServicesSpec : Specification<OfferedService>
    {
        public PaginatedOfferedServicesSpec(string freelancerId, int pageSize, int pageIndex)
            : base(os => os.FreelancerId == freelancerId)
        {
            ApplyPaging(pageIndex, pageIndex * pageSize);
        }
    }
}
