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
        public int Id { get; set; }
        public int RoomID { get; set; }
        public Room Room { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set;}
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public bool IsAvialable { get; set; }

    }
}
