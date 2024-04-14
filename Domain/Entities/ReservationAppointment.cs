using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Models
{
    public class ReservationAppointment
    {
        public string Id { get; set; }
        public string RoomID { get; set; }
        public virtual Room Room { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set;}
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public bool IsAvialable { get; set; }

    }
}
