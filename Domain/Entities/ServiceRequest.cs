﻿using Connect.Core.Entities;

namespace Connect.Core.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public RequisStatus Status { get; set; }
        public decimal Price { get; set; }
        public int FreelanceId { get; set; }
        public Freelancer Freelancer { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
       

    }

    public enum RequisStatus
    {
        Pending,
        Accepted,
        Refused,
        Completed
        

    }
}
