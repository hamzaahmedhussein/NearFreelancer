using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Connect.Infrastructure.Repsitory_UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public virtual IGenericRepository<Customer> Customer { get; set; }
        public virtual IGenericRepository<ReservationProvider> ReservationBusiness { get; set; }
        public virtual IGenericRepository<Freelancer> FreelancerBusiness { get; set; }
        public virtual IGenericRepository<OfferedService> OfferedService { get; set; }
        public virtual IGenericRepository<ServiceRequest> ServiceRequest { get; set; }


        private IDbContextTransaction transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customer = new GenericRepository<Customer>(_context);
            ReservationBusiness = new GenericRepository<ReservationProvider>(_context);
            FreelancerBusiness = new GenericRepository<Freelancer>(_context);
            OfferedService = new GenericRepository<OfferedService>(_context);
            ServiceRequest = new GenericRepository<ServiceRequest>(_context);
        }

        public void CreateTransaction()
        {
            transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();

        }


        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
