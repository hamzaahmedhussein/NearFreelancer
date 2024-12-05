using Connect.Core.Entities;
using Connect.Core.Models;
using Connect.Infrastructure.Repsitory_UOW;

namespace Connect.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Customer> Customer { get; }
        public IGenericRepository<ServiceRequest> ServiceRequest { get; }
        public IGenericRepository<ReservationProvider> ReservationBusiness { get; }
        public IGenericRepository<Freelancer> FreelancerBusiness { get; }
        public IGenericRepository<OfferedService> OfferedService { get; set; }
        public IChatRepository ChatRepository { get; set; }


        void CreateTransaction();
        void Commit();
        void Rollback();
        int Save();

    }
}
