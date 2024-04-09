﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class AddFreelancerBusinessDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public String PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public List<string> Skills { get; set; }
        public bool Availability { get; set; }
    }
}
