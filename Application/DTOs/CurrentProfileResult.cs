using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class CurrentProfileResult
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public List<ServiceRequest> Requests { get; set; }
        public List<ReservationAppointment> Reservations { get; set; }

    }
}
