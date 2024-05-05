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
        public FreelancerWithOfferedServices(string id, int pageNumber)
            : base(c => c.Id == id)
        {
            AddInclude(c => c.OfferedServices);

            int pageSize = 4;  
            int skipCount = (pageNumber - 1) * pageSize;
            ApplyPaging(pageSize, skipCount);
        }
    }
}
