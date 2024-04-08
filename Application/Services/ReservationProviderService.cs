using AutoMapper;
using Connect.Application.DTOs;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Connect.Application.Services
{
    public class ReservationProviderService:IReservationProviderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly UserManager<Customer> _userManager;

        public ReservationProviderService(IUnitOfWork unitOfWork,
            IConfiguration config, IMapper mapper,
            IUserHelpers userHelpers,
            UserManager<Customer> userManager )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _userManager = userManager;
        }

        public async Task<bool> AddReservationBusiness(AddReservationBusinessDto reservationDto)
        {
            if (reservationDto == null)
                return false;

            var user = await _userHelpers.GetCurrentUserAsync();

            if (user == null)
                throw new Exception("User not found.");

            if (await _userManager.IsInRoleAsync(user, "ReservationProvider"))
                throw new Exception("User already has a ReservationProvider profile.");

            var result = await _userManager.AddToRoleAsync(user, "ReservationProvider");
            if (!result.Succeeded)
                throw new Exception("Failed to assign ReservationProvider role to the user.");

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
