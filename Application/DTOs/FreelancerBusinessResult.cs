using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class FreelancerBusinessResult
    {
        public string Name { get; set; }

        public string Description { get; set; }
     
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public string Location { get; set; }
        public List<OfferedService> OfferedServicesList { get; set; }
        public List<string> Skills { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
    }
}
