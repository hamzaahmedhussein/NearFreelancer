using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class SystemStatisticsDto
    {
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int TotalFreelancers { get; set; }
        public int NewFreelancers { get; set; }
        public int TotalServiceRequests { get; set; }
        public int NewServiceRequests { get; set; }
        public decimal TotalRevenue { get; set; }
        public int AcceptedRequests { get; set; }
    }
}
