using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Entities
{
    public class OfferedService
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public DateTime DOJ { get; set; }
        public bool IsAvailable { get; set; }
    }
}
