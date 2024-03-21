using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IFreelancerService _freelanceService;
        private readonly IReservationProviderService _reservationProviderService;


        public AccountController(ICustomerService customerService, IFreelancerService freelance, IReservationProviderService reservationProviderService)
        {
            _customerService = customerService;
            _freelanceService = freelance;
            _reservationProviderService = reservationProviderService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _customerService.Register(userDto);
            if (result.Succeeded)
            {
                return Ok("Registration succeeded.");
            }

            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _customerService.Login(userDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized("Invalid login attempt.");
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var result = await _customerService.GetCurrentProfileAsync();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound("User profile not found.");
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }

            var result = await _customerService.SendPasswordResetEmail(email);
            return result ? Ok("Password reset email sent.") : BadRequest("User not found.");
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _customerService.ResetPassword(resetDto);
            if (result)
            {
                return Ok("Password reset successfully.");
            }
            else
            {
                return BadRequest("Password reset failed.");
            }
        }
        [HttpPost("get-freelancer")]
        public IActionResult GetFreelancer(int id)
        {
            var model = _freelanceService.GetFreelancerById(id);
            return Ok(model);
        }

        [HttpPost("get-reservationProvider")]
        public IActionResult GetReservationProvider(int id)
        {
            var model = _reservationProviderService.GetReservationProviderById(id);
            return Ok(model);
        }



    }


}

