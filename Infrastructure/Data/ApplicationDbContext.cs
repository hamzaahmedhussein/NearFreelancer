using Connect.Core.Entities;
using Connect.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
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
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; } 
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                  new IdentityRole() { Name = "Customer", ConcurrencyStamp = "0", NormalizedName = "Customer" },
                  new IdentityRole() { Name = "Freelancer", ConcurrencyStamp = "1", NormalizedName = "Freelancer" },
                  new IdentityRole() { Name = "ReservationProvider", ConcurrencyStamp = "2", NormalizedName = "ReservationProvider" }

                );
           
        }
    }
}
