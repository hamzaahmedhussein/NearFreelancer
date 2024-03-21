using AutoMapper;
using Connect.Application.DTOs;
using Connect.Application.Helpers;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Connect.Application.Services
{
    public class ReservationProviderService:IReservationProviderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        public ReservationProviderService(IUnitOfWork unitOfWork,
            IConfiguration config, IMapper mapper,
            IUserHelpers userHelpers
           )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }

        public async Task<bool> AddResevationBusiness(AddReservationBusinessDto reservationDto)
        {

            if (reservationDto == null)
            {
                return false;
            }
            var user = await _userHelpers.GetCurrentUserAsync();


            if (user == null || user.ProfileType == ProfileType.ReservationProvider)
            {
                return false;
            }

            _userHelpers.ChangeUserTypeAsync(4, user);

            var reservationProvider = _mapper.Map<ReservationProvider>(reservationDto);
            reservationProvider.Owner = user;

            user.ReservationProvider = reservationProvider;
            _unitOfWork.Save();

            return true;
        }

        public async Task<ReservationBusinessResult> GetReservationProfile()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user != null)
            {
                var profile = user.ReservationProvider;
                if (profile != null)
                {
                    return _mapper.Map<ReservationBusinessResult>(profile);

                }
            }

            return null;
        }

        public async Task<ReservationBusinessResult> GetReservationProviderById(int id)
        {
            var profile = _unitOfWork.ReservationBusiness.GetById(id);
            return _mapper.Map<ReservationBusinessResult>(profile);
        }


    }
}
