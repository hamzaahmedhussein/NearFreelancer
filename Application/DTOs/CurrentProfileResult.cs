using Connect.Core.Models;
namespace Connect.Application.DTOs
{
    public class CurrentProfileResult
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public List< ServiceRequest> Requests { get; set; }
        public List<ReservationAppointment> Reservations { get; set; }

    }
}
