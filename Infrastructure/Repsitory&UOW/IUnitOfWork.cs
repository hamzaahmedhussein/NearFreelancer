using Connect.Core.Entities;
using Connect.Core.Models;

namespace Connect.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Customer> Customer { get; }
        public IGenericRepository<ServiceRequest> ServiceRequest { get; }
        public IGenericRepository<ReservationProvider> ReservationBusiness { get; }
        public IGenericRepository<Freelancer> FreelancerBusiness { get;  }
        public IGenericRepository<OfferedService> OfferedService { get; set; }
        int Save();

    }
}
