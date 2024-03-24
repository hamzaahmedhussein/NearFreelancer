using Connect.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    public interface IReservationProviderService
    {
        Task<bool> AddReservationBusiness(AddReservationBusinessDto reservationDto);
        Task<ReservationBusinessResult> GetReservationProfile();
        Task<ReservationBusinessResult> GetReservationProviderById(int id);

    }
}
