using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class FreelancerProfileResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Image { get; set; }

        public string Description { get; set; }
      
        public string Location { get; set; }
        public List<string> Skills { get; set; }
        public bool Availability { get; set; }
    }
}
