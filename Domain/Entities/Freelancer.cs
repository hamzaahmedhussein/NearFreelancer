using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Models
{
    public class Freelancer
    {
        public string Id { get; set; }=Guid.NewGuid().ToString();
        public string Name { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Profession { get; set; }
        public string? Description { get; set; }
        public string? OwnerId { get; set; }
        public virtual Customer? Owner { get; set; }
      //  public List<int>? EmployeesId { get; set; } 
        public string? Image { get; set; }
        public string? BackgroundImage { get; set; }
        public string? Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual List<Message>? SentMessages { get; set; }
        public virtual List<Message>? ReceivedMessages { get; set; }
        public virtual List<OfferedService>? OfferedServicesList { get; set; }
       // public virtual List<ServiceRequest>? Requests { get; set; }
        public DateTime DOJ { get; set; }
        public virtual List<string>? Skills { get; set; }
        public bool? Availability { get; set; }

    }
}
