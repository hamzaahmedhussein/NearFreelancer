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
        public string? Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DOJ { get; set; }
        public DateTime? DOB { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }
        public virtual ICollection<ServiceRequest> Requests { get; set; }
        public virtual ICollection<ReservationAppointment> Reservations { get; set; }

        public virtual Freelancer Freelancer { get; set; }

        public virtual ReservationProvider ReservationProvider { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
    }

  
}
