using Connect.Application.DTOs;
using Connect.Application.Services;
using Connect.Application.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Constructor
        private readonly ICustomerService _customerService;
        private readonly IAdminService _adminService;
        private readonly IMailingService _mailingService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ICustomerService customerService,IMailingService mailingService, ILogger<AccountController> logger, IAdminService adminService)
        {
            _customerService = customerService;
            _mailingService = mailingService;
            _logger = logger;
            _adminService = adminService;
        }
        #endregion

        #region Registeration


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state in registration request.");
                return BadRequest(ModelState);
            }

            try
            {
                Log.Information("Processing registration request for email: {Email}", userDto.Email);

                var result = await _customerService.Register(userDto);

                if (result.Succeeded)
                {
                    Log.Information("Registration succeeded for email: {Email}", userDto.Email);
                    return Ok("Registration succeeded.");
                }

                var errorDescription = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
                Log.Warning("Registration failed for email: {Email}. Error: {Error}", userDto.Email, errorDescription);

                return BadRequest(errorDescription);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred during registration for email: {Email}", userDto.Email);

                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state in login request.");
                return BadRequest(ModelState);
            }

            try
            {
                Log.Information("Processing login request for email: {Email}", userDto.Email);

                var result = await _customerService.Login(userDto);

                if (result.Success)
                {
                    Log.Information("Login successful for email: {Email}", userDto.Email);
                    return Ok(result);
                }

                (string errorMessage, int statusCode) = result.ErrorType switch
                {
                    LoginErrorType.UserNotFound => ("User not found.", 404),
                    LoginErrorType.InvalidPassword => ("Incorrect password.", 401),
                    LoginErrorType.EmailNotConfirmed => ("Email not confirmed.", 403),
                    _ => ("Invalid login attempt.", 400)
                };

                Log.Warning("Login failed for email: {Email}. Error: {Error}", userDto.Email, errorMessage);

                return StatusCode(statusCode, new { message = errorMessage });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the login request for email: {Email}", userDto.Email);
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var success = await _customerService.ConfirmEmail(email, token);
            if (success)
                return RedirectToAction("Login");

            return BadRequest("Failed to confirm email.");
        }
       

        [HttpPost("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var result = await _customerService.LogoutAsync();
            if (result == "Logged Out Successfully")
                return Ok(result);
            return BadRequest(result);
        }
        #endregion

        [HttpGet("user-roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var roles = await _customerService.GetUserRolesAsync();
            if (roles != null)
            {
                return Ok(roles);
            }
            else
            {
                _logger.LogError("Failed to retrieve roles for the current user.");
                return StatusCode(500, "Failed to retrieve roles for the current user.");
            }
        }


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


        [HttpGet("get-customer-by-id/{id}")]
        public async Task<IActionResult> GetFreelancerById(string id)
        {
            try
            {
                _logger.LogInformation("API request: Getting customer profile for ID: {CustomerId} with requests ", id);

                var result = await _customerService.GetCustomerById(id);

                if (result == null)
                {
                    _logger.LogWarning("Freelancer with ID: {customerId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API error: Error retrieving customer profile for ID: {customerId}", id);

                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        //[HttpPost("get-reservationProvider")]
        //public IActionResult GetReservationProvider(string id)
        //{
        //    var model = _reservationProviderService.GetReservationProviderById(id);
        //    return Ok(model);
        //}


        #region Forgot Password
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var result = await _customerService.ForgotPasswordAsync(Email);
            if (result == "OTP Sent Successfully")
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPDto Model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _customerService.VerifyOTPAsync(Model);
            if (result != "Invalid OTP")
                return Ok(result);
            return BadRequest(result);
        }

    
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto Model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _customerService.ResetPasswordAsync(Model);
            if (result == "Password Reset Successfully")
                return Ok(result);
            return BadRequest(result);
        }
        #endregion


        #region Refresh Token
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Invalid Token");
            }
            var result = await _customerService.RefreshTokenAsync();
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            _customerService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] string Token)
        {
            Token = Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(Token))
            {
                return BadRequest("Token is Required");
            }
            var result = await _customerService.RevokeTokenAsync(Token);
            if (result)
            {
                return Ok("Token Revoked Successfully");
            }
            return BadRequest("Token Not Revoked");
        }



        #endregion


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


        [HttpPost("send-service-request")]
        public async Task<IActionResult> SendServiceRequest(string freelancerId, [FromBody] SendServiceRequestDto requestDto)
        {
            var result = await _customerService.SendServiceRequist(freelancerId, requestDto);

            if (result)
                return Ok();
            else
                return BadRequest("Failed to send service request.");
        }

        [HttpGet("get-requests")]
        public async Task<IActionResult> GetMyRequests(int pageIndex, int pageSize)
        {
            try
            {
                var requests = await _customerService.GetMyRequests(  pageIndex , pageSize);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-customer-info")]
        public async Task<IActionResult> UpdateCustomerInfo([FromBody] UpdateCustomerInfoDto updateDto)
        {
            var success = await _customerService.UpdateCustomerInfo(updateDto);

            if (!success)
            {
                return BadRequest("Failed to update customer information.");
            }

            return Ok("Customer information updated successfully.");
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var success = await _customerService.DeleteAccountAsync();

            if (!success)
            {
                return NotFound(); 
            }

            return Ok(); 
        }

        [HttpPost]
        [Route("api/delete-request/{id}")]
        public async Task<IActionResult> DeleteRequest(string id)
        {
            try
            {
                var result = await _customerService.DeletePendingRequestAsync(id);

                if (result)
                {
                    return Ok("Request deleted successfully.");
                }
                else
                {
                    return NotFound("Request not found or you don't have permission to delete it.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        #region file handling
        [Authorize]
        [HttpGet("get-customer-picture")]
        public async Task<IActionResult> GetCustomerPictureAsync()
        {
            var result = await _customerService.GetCustomerPictureAsync();
            return result != string.Empty ? Ok(result) : BadRequest("there is not picture.");
        }
        //[Authorize]
        //[HttpPost("add-customer-picture")]
        //public async Task<IActionResult> AddCustomerPictureAsync(IFormFile? file)
        //{
        //    var result = await _customerService.AddCustomerPictureAsync(file);
        //    return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        //}
        [Authorize]
        [HttpPut("Update-customer-picture")]
        public async Task<IActionResult> UpdateCustomerPictureAsync(IFormFile? file)
        {
            var result = await _customerService.UpdateCustomerPictureAsync(file);
            return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        }
        [Authorize]
        [HttpDelete("delete-customer-picture")]
        public async Task<IActionResult> DeleteCustomerPictureAsync()
        {
            var result = await _customerService.DeleteCustomerPictureAsync();
            return result ? Ok("picture has been deleted successfully.") : BadRequest("failed to delete picture");
        }
        #endregion



        [HttpGet("last-month-statistics")] 
        public async Task<IActionResult> GetLastMonthStatisticsAsync()
        {
            var statistics = await _adminService.GetLastMonthStatisticsAsync();
            return Ok(statistics); 
        }
    }

}




