using Connect.Core.Entities;
using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class AddReservationBusinessDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public String PhoneNumber { get; set; }
        public string Location { get; set; }
        public List<string> FeatureList { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
    }
}
