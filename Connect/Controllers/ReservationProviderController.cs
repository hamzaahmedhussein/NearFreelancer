using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationProviderController : ControllerBase
    {
        private readonly IReservationProviderService _reservationProviderService;

        public ReservationProviderController(IReservationProviderService reservationProviderService)
        {
            _reservationProviderService = reservationProviderService;
        }
        [Authorize]
        [HttpPost("add-reservation-business")]
        public async Task<IActionResult> AddReservationBusiness([FromBody] AddReservationBusinessDto reservationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var isCreated = await _reservationProviderService.AddReservationBusiness(reservationDto);
                return isCreated ? Ok("Reservation provider created successfully.") : BadRequest("Failed to create reservation provider.");
            }
            catch (Exception ex)
            {
                return BadRequest("Already has Reservation Provider");
            }
        }

        [HttpGet("reservation-profile")]
        public async Task<IActionResult> GetReservationProfile()
        {
            var result = await _reservationProviderService.GetReservationProfile();
            return result != null ? Ok(result) : NotFound("Reservation profile not found.");
        }

    }
}
