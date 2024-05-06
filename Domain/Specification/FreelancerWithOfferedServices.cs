using Connect.Core.Entities;
using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Specification
{
    public class FreelancerWithOfferedServices : Specification<Freelancer>
    {
        public FreelancerWithOfferedServices(string freelancerId, int pageNumber, int pageSize = 4)
            : base(freelancer => freelancer.Id == freelancerId)
        {
            // Add include for offered services
            AddInclude(freelancer => freelancer.OfferedServices);

            // Calculate the skip count
            int skipCount = (pageNumber - 1) * pageSize;

            // Apply pagination
            ApplyPaging(pageSize, skipCount);
        }
    }

}
