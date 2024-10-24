using Connect.Application.DTOs;
using Connect.Application.Services;
using Connect.Application.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        public AccountController(ICustomerService customerService, IMailingService mailingService, ILogger<AccountController> logger, IAdminService adminService)
        {
            _customerService = customerService;
            _mailingService = mailingService;
            _logger = logger;
            _adminService = adminService;
        }
        #endregion

        #region Registration

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state in registration request.");
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid model state.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                Log.Information("Processing registration request for email: {Email}", userDto.Email);

                var result = await _customerService.Register(userDto);

                if (result.Succeeded)
                {
                    Log.Information("Registration succeeded for email: {Email}", userDto.Email);
                    return Ok(new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Registration succeeded.",
                        Data = null
                    });
                }

                var errorDescription = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
                Log.Warning("Registration failed for email: {Email}. Error: {Error}", userDto.Email, errorDescription);

                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = errorDescription,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred during registration for email: {Email}", userDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred. Please try again later.",
                    Data = null
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginUserDto>>> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state in login request.");
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid model state.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                Log.Information("Processing login request for email: {Email}", userDto.Email);

                var result = await _customerService.Login(userDto);

                if (result.Success)
                {
                    Log.Information("Login successful for email: {Email}", userDto.Email);
                    return Ok(new ApiResponse<LoginResult>
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Login successful.",
                        Data = result
                    });
                }

                (string errorMessage, int statusCode) = result.ErrorType switch
                {
                    LoginErrorType.UserNotFound => ("User not found.", StatusCodes.Status404NotFound),
                    LoginErrorType.InvalidPassword => ("Incorrect password.", StatusCodes.Status401Unauthorized),
                    LoginErrorType.EmailNotConfirmed => ("Email not confirmed.", StatusCodes.Status403Forbidden),
                    _ => ("Invalid login attempt.", StatusCodes.Status400BadRequest)
                };

                Log.Warning("Login failed for email: {Email}. Error: {Error}", userDto.Email, errorMessage);

                return StatusCode(statusCode, new ApiResponse<string>
                {
                    StatusCode = statusCode,
                    Message = errorMessage,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the login request for email: {Email}", userDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred. Please try again later.",
                    Data = null
                });
            }
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<ApiResponse<string>>> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var success = await _customerService.ConfirmEmail(email, token);
            if (success)
                return Ok(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Email confirmed successfully.",
                    Data = null
                });

            return BadRequest(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Failed to confirm email.",
                Data = null
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            var result = await _customerService.LogoutAsync();
            if (result == "Logged Out Successfully")
                return Ok(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result,
                    Data = null
                });

            return BadRequest(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = result,
                Data = null
            });
        }

        #endregion

        [HttpGet("user-roles")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetUserRoles()
        {
            var roles = await _customerService.GetUserRolesAsync();
            if (roles != null)
            {
                return Ok(new ApiResponse<IEnumerable<string>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Roles retrieved successfully.",
                    Data = roles
                });
            }
            else
            {
                Log.Error("Failed to retrieve roles for the current user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Failed to retrieve roles for the current user.",
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var result = await _customerService.GetCurrentProfileAsync();
            if (result != null)
            {
                return Ok(new ApiResponse<CustomerProfileResult>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Profile retrieved successfully.",
                    Data = result
                });
            }

            return NotFound(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "User profile not found.",
                Data = null
            });
        }

        [HttpGet("get-customer-by-id/{id}")]
        public async Task<ActionResult<ApiResponse<CustomerProfileResult>>> GetCustomerById(string id)
        {
            try
            {
                Log.Information("API request: Getting customer profile for ID: {CustomerId}", id);

                var result = await _customerService.GetCustomerById(id);

                if (result == null)
                {
                    Log.Warning("Customer with ID: {customerId} not found", id);
                    return NotFound(new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Customer not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<CustomerProfileResult>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Customer retrieved successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "API error: Error retrieving customer profile for ID: {customerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }


        #region Forgot Password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _customerService.ForgotPasswordAsync(email);
            return result == "OTP Sent Successfully" ? Ok(result) : BadRequest(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPDto model)
        {
            if (HandleModelState() is IActionResult badRequestResult)
                return badRequestResult;

            var result = await _customerService.VerifyOTPAsync(model);
            return result != "Invalid OTP" ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (HandleModelState() is IActionResult badRequestResult)
                return badRequestResult;

            var result = await _customerService.ResetPasswordAsync(model);
            return result == "Password Reset Successfully" ? Ok(result) : BadRequest(result);
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
        public async Task<IActionResult> RevokeTokenAsync([FromBody] string token)
        {
            token = token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is Required");
            }

            var result = await _customerService.RevokeTokenAsync(token);
            return result ? Ok("Token Revoked Successfully") : BadRequest("Token Not Revoked");
        }
        #endregion

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (HandleModelState() is IActionResult badRequestResult)
                return badRequestResult;

            var changePasswordResult = await _customerService.ChangePassword(changePasswordDto);
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
            return result ? Ok() : BadRequest("Failed to send service request.");
        }

        [HttpGet("get-requests")]
        public async Task<IActionResult> GetMyRequests(int pageIndex, int pageSize)
        {
            try
            {
                var requests = await _customerService.GetMyRequests(pageIndex, pageSize);

                return Ok(new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Requests retrieved successfully",
                    Data = requests
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = "Failed to retrieve requests",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        [HttpPut("update-customer-info")]
        public async Task<IActionResult> UpdateCustomerInfo([FromBody] UpdateCustomerInfoDto updateDto)
        {
            var success = await _customerService.UpdateCustomerInfo(updateDto);
            return success ? Ok("Customer information updated successfully.") : BadRequest("Failed to update customer information.");
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var success = await _customerService.DeleteAccountAsync();
            return success ? Ok() : NotFound();
        }

        [HttpPost("api/delete-request/{id}")]
        public async Task<IActionResult> DeleteRequest(string id)
        {
            try
            {
                var result = await _customerService.DeletePendingRequestAsync(id);
                return result ? Ok("Request deleted successfully.") : NotFound("Request not found or you don't have permission to delete it.");
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

        #region File Handling
        [Authorize]
        [HttpGet("get-customer-picture")]
        public async Task<IActionResult> GetCustomerPictureAsync()
        {
            var result = await _customerService.GetCustomerPictureAsync();
            return !string.IsNullOrEmpty(result) ? Ok(result) : BadRequest("There is no picture.");
        }

        [Authorize]
        [HttpPut("update-customer-picture")]
        public async Task<IActionResult> UpdateCustomerPictureAsync(IFormFile? file)
        {
            var result = await _customerService.UpdateCustomerPictureAsync(file);
            return result ? Ok("Picture has been updated successfully.") : BadRequest("Failed to update picture.");
        }

        [Authorize]
        [HttpDelete("delete-customer-picture")]
        public async Task<IActionResult> DeleteCustomerPictureAsync()
        {
            var result = await _customerService.DeleteCustomerPictureAsync();
            return result ? Ok("Picture has been deleted successfully.") : BadRequest("Failed to delete picture.");
        }
        #endregion

        [HttpGet("last-month-statistics")]
        public async Task<IActionResult> GetLastMonthStatisticsAsync()
        {
            var statistics = await _adminService.GetLastMonthStatisticsAsync();
            return Ok(statistics);
        }

        #region Helper Methods
        private IActionResult HandleModelState()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return null;
        }
        #endregion
    }
}




