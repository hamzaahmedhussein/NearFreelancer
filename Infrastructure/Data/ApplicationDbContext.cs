using Connect.Core.Entities;
using Connect.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }   

        public DbSet<Customer> Customers { get; set; }
        public DbSet<OfferedService> OfferedServices { get; set; }
        public DbSet<ReservationProvider> ReservationProviders { get; set; } 
        public DbSet<ReservationAppointment> Reservations { get; set; }
        public DbSet<Freelancer> ServiceProviders { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; } 
        public DbSet<Room> Rooms { get; set; }
    }
}
