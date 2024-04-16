using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Specification
{
    public class CustomerWithFreelancer : Specification<Customer>
    {
        public CustomerWithFreelancer(string id)
            :base(c=>c.Id==id)
        {
            AddInclude(c=>c.Freelancer);
        }
    }
}
