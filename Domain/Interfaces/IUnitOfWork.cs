using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect.Core.Entities;
using Connect.Core.Models;

namespace Connect.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Customer> Customer { get; }
        public IGenericRepository<ReservationProvider> ReservationBusiness { get; }
        public IGenericRepository<Freelancer> FreelancerBusiness { get;  }
        public IGenericRepository<OfferedService> OfferedService { get; set; }


        int Save();

    }
}
