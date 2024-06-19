using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class VerifyOTPDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}
