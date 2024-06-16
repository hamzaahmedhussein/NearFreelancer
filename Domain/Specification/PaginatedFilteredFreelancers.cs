using Connect.Core.Entities;
using Connect.Core.Models;
using Connect.Core.Specification;
using System;
using System.Linq.Expressions;

namespace Connect.Application.Specifications
{
    public class PaginatedFilteredFreelancers : Specification<Freelancer>
    {
        public PaginatedFilteredFreelancers(string search, int pageIndex, int pageSize)
            : base(BuildSearchExpression(search))
        {
            ApplyPaging(pageSize, (pageIndex - 1) * pageSize);
        }

        private static Expression<Func<Freelancer, bool>> BuildSearchExpression(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return f => true; 
            }
            return f =>
                f.Name.Contains(search) ||
                f.Profession.Contains(search) ||
                (f.City != null && f.City.Contains(search)) ||
                (f.Street != null && f.Street.Contains(search));
        }
    }
}
