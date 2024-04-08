using System;
using System.Collections.Generic;
using Connect.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Connect.Core.Entities
{
    public class Customer : IdentityUser
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? BackgroundImage { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DOJ { get; set; }
        public DateTime? DOB { get; set; }
        public List<ServiceRequest> Requests { get; set; }
        public List<ReservationAppointment> Reservations { get; set; }

        public Freelancer Freelancer { get; set; }

        public ReservationProvider ReservationProvider { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
    }

  
}
