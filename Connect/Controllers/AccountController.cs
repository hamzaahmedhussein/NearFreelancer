﻿using Connect.Application.DTOs;
using Connect.Application.MailSettings;
using Connect.Application.Services;
using Connect.Application.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IFreelancerService _freelanceService;
        private readonly IReservationProviderService _reservationProviderService;
        private readonly IMailingService _mailingService;



        public AccountController(ICustomerService customerService, IFreelancerService freelance, IReservationProviderService reservationProviderService,IMailingService mailingService)
        {
            _customerService = customerService;
            _freelanceService = freelance;
            _reservationProviderService = reservationProviderService;
            _mailingService = mailingService;
        }


        #region Registeration

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

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var success = await _customerService.ConfirmEmail(email, token);
            if (success)
                return Ok("Email confirmed successfully.");

            return BadRequest("Failed to confirm email.");
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.Login(userDto);
            if (result.Success)
                return Ok(result);

            string errorMessage;
            if (result.ErrorType == LoginErrorType.UserNotFound)
            {
                errorMessage = "User not found.";
            }
            else if (result.ErrorType == LoginErrorType.InvalidPassword)
            {
                errorMessage = "Incorrect password.";
            }
            else
            {
                errorMessage = "Invalid login attempt.";
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return Unauthorized(ModelState);
        }

        #endregion 


        [Authorize]
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


        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isSent = await _customerService.ForgetPassword(email);
            if (isSent)
            {
                return Ok("Reset password email sent successfully.");
            }
            else
            {
                return NotFound("User with provided email not found.");
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token,string email)
        {
            var model = new ResetPasswordDto { Token = token, Email = email };

            return Ok(new { model });
        }
            
            
            
            [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resetPasswordResult = await _customerService.ResetPassword(resetPasswordDto);
            if (!resetPasswordResult.Succeeded)
            {
                foreach (var error in resetPasswordResult.Errors)
                   ModelState.AddModelError(error.Code,error.Description);

                return BadRequest(ModelState);
            }

            return Ok("Password reset successfully");

        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var changePasswordResult = await _customerService.ChangePassword( changePasswordDto);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok("Password changed successfully");
        }


    }


}

