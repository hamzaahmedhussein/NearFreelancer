using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.DTOs
{
    public class RefreshTokenResult
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public bool IsAuthenticated { get; set; }
    }

}
