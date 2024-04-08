using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class FreelancerFilterResultDto
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string? Image { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
