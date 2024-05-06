using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class ServiceRequestResult
    {
        public string? Name { get; set; }
        public RequisStatus Status { get; set; }
        public  RequestedFreelancerDto Freelancer { get; set; }

    }
}
