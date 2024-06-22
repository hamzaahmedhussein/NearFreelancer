using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class FilterFreelancersDto
    {
        public string Name { get; set; }
        public string Profession { get; set; }
       
        public
             List<string> Skills { get; set; }
        public bool Availability { get; set; }
    }
}
