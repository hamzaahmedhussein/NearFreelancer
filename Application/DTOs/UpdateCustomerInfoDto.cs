using Connect.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class UpdateCustomerInfoDto
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
    }
}
