using Connect.Core.Entities;
using Connect.Core.Models;
namespace Connect.Application.DTOs
{
    public class ReservationBusinessResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
     
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public string location { get; set; }
        public List<OfferedService> OfferedServicesList { get; set; }
        public List<string> FeatureList { get; set; }
        public List<Room> Rooms { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
    }
}
