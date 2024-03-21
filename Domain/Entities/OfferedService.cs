using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Core.Entities
{
    public class OfferedService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descrioption { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public int ProviderId { get; set; }
        //ask about the nav proberties is required here or not
        public DateTime DOJ { get; set; }
        public bool IsAvailable { get; set; }
    }
}
