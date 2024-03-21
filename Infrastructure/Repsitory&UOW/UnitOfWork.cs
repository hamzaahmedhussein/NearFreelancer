using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Infrastructure.Repsitory_UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IGenericRepository<Customer> Customer { get; set; }
        public IGenericRepository<ReservationProvider> ReservationBusiness { get; set; }
        public IGenericRepository<Freelancer> FreelancerBusiness { get; set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customer = new GenericRepository<Customer>(_context);
        }

       

        public int Save()
        {
           return _context.SaveChanges();
        }
    }
}
