using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Models
{
    public class Room
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public double CostPerNight { get; set; }
        public string ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public virtual ReservationProvider Provider { get; set; }
        public bool IsAvailable { get; set; }
        public int BedsNumber { get; set; }
        public List<string> FeatureList { get; set; }
    }
}
