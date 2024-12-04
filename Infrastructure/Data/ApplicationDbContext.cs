using Connect.Core.Entities;
using Connect.Core.Models;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
               .Property(s => s.Image)
               .HasDefaultValue("/Images/default/avatar")
               .IsRequired();


            builder.Entity<Freelancer>()
               .Property(s => s.Image)
               .HasDefaultValue("/Images/default/avatar")
               .IsRequired();




            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                  new IdentityRole
                  {
                      Id = "0",
                      Name = "Admin",
                      ConcurrencyStamp = Guid.NewGuid().ToString(),
                      NormalizedName = "Admin"
                  }
                ,
                new IdentityRole
                {
                    Id = "1",
                    Name = "Customer",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    NormalizedName = "Customer"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Freelancer",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    NormalizedName = "Freelancer"
                }

            );
        }
    }
}
