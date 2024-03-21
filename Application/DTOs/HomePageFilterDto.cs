using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class HomePageFilterDto
    {
        public ProviderType ProviderType { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<string> Capability { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
    }
   public enum ProviderType
    {
        ReservationProvider,
        Freelancer

    }
}

