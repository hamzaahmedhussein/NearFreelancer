using Connect.Core.Entities;
using Connect.Core.Specification;

namespace Connect.Core.Specification
{
    public class CustomerWithFreelancerSpec : Specification<Customer>
    {
        public CustomerWithFreelancerSpec(string id)
            : base(c=>c.Id==id)
        {
            AddInclude(c => c.Freelancer);
        }
    }
}
