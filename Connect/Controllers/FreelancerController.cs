using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController : ControllerBase
    {
        private readonly IFreelancerService _freelancerService;
        private readonly ILogger<FreelancerService> _logger;


        public FreelancerController(IFreelancerService freelancerService, ILogger<FreelancerService> logger)
        {
            _freelancerService = freelancerService ?? throw new ArgumentNullException(nameof(freelancerService));
            _logger = logger;
        }
        [Authorize]
        [HttpPost("add-freelancer-business")]
        public async Task<IActionResult> AddFreelancerBusiness([FromBody] AddFreelancerBusinessDto freelancerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid model state.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            try
            {
                var isCreated = await _freelancerService.AddFreelancerBusiness(freelancerDto);
                return isCreated
                    ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancer created successfully." })
                    : BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Failed to create freelancer." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete-freelancer-business")]
        public async Task<IActionResult> DeleteFreelancerBusiness()
        {
            try
            {
                var isDeleted = await _freelancerService.DeleteFreelancerBusinessAsync();
                return isDeleted
                    ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancer deleted successfully." })
                    : BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Failed to delete freelancer." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update-freelancer-business")]
        public async Task<IActionResult> UpdateFreelancerBusiness([FromBody] AddFreelancerBusinessDto freelancerDto)
        {
            try
            {
                var result = await _freelancerService.UpdateFreelancerBusiness(freelancerDto);
                return result
                    ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancer updated successfully." })
                    : BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Failed to update freelancer." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("freelancer-profile")]
        public async Task<IActionResult> GetFreelancerProfile()
        {
            var result = await _freelancerService.GetFreelancerProfile();
            return result != null
                ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancer profile retrieved successfully.", Data = result })
                : NotFound(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.NotFound, Message = "Freelancer profile not found." });
        }

        [HttpGet("get-freelancer-by-id/{id}")]
        public async Task<IActionResult> GetFreelancerById(string id)
        {
            try
            {
                _logger.LogInformation("API request: Getting freelancer profile for ID: {FreelancerId} with offered services", id);
                var result = await _freelancerService.GetFreelancerById(id);

                if (result == null)
                {
                    _logger.LogWarning("Freelancer with ID: {FreelancerId} not found", id);
                    return NotFound(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.NotFound, Message = "Freelancer not found." });
                }

                return Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancer profile retrieved successfully.", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API error: Error retrieving freelancer profile for ID: {FreelancerId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = "An error occurred while processing your request." });
            }
        }

        [Authorize]
        [HttpPost("add-offered-service")]
        public async Task<IActionResult> AddOfferedService(AddOfferedServiceDto serviceDto)
        {
            try
            {
                var result = await _freelancerService.AddOfferedService(serviceDto);
                return result
                    ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Offered service added successfully." })
                    : BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Failed to add offered service." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update-offered-service/{id}")]
        public async Task<IActionResult> UpdateOfferedService(string id, AddOfferedServiceDto serviceDto)
        {
            try
            {
                var result = await _freelancerService.UpdateOfferedService(id, serviceDto);
                return result
                    ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Offered service updated successfully." })
                    : BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Failed to update offered service." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost("filter-freelancers")]
        public async Task<IActionResult> FilterFreelancers(string? search, int pageIndex, int pageSize = 9)
        {
            try
            {
                var result = await _freelancerService.FilterFreelancers(search, pageIndex, pageSize);
                return Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancers filtered successfully.", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("get-freelancer-requests")]
        public async Task<IActionResult> GetFreelancerRequests(int pageIndex, int pageSize = 4)
        {
            try
            {
                var requests = await _freelancerService.GetFreelancerRequests(pageIndex, pageSize);
                return Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Freelancer requests retrieved successfully.", Data = requests });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("accept/{requestId}")]
        public async Task<IActionResult> AcceptRequest(string requestId)
        {
            try
            {
                var result = await _freelancerService.AcceptServiceRequest(requestId);
                return result
                    ? Ok(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.OK, Message = "Request accepted successfully." })
                    : BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Failed to accept request." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("refuse/{requestId}")]
        public async Task<IActionResult> RefuseRequest(string requestId)
        {
            try
            {
                var result = await _freelancerService.RefuseServiceRequest(requestId);
                return result
                    ? Ok(new ApiResponse<string> { StatusCode = 200, Message = "Request refused successfully.", Data = null })
                    : BadRequest(new ApiResponse<string> { StatusCode = 400, Message = "Failed to refuse request." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string> { StatusCode = 500, Message = ex.Message, Errors = new List<string> { ex.ToString() } });
            }
        }

        [Authorize]
        [HttpGet("get-offered-services/{freelancerId}")]
        public async Task<IActionResult> GetOfferedService(string freelancerId, int pageIndex, int pageSize = 4)
        {
            if (string.IsNullOrWhiteSpace(freelancerId))
            {
                return BadRequest(new ApiResponse<string> { StatusCode = 400, Message = "Freelancer ID must be provided." });
            }

            var offeredServiceResults = await _freelancerService.GetOfferedServicesAsync(freelancerId, pageIndex, pageSize);
            return Ok(new ApiResponse<IEnumerable<OfferedServiceResult>> { StatusCode = 200, Message = "Offered services retrieved successfully.", Data = offeredServiceResults });
        }

        #region File Handling
        [Authorize]
        [HttpGet("get-freelancer-picture")]
        public async Task<IActionResult> GetUserPictureAsync()
        {
            var result = await _freelancerService.GetFreelancerPictureAsync();
            return result != string.Empty
                ? Ok(new ApiResponse<string> { StatusCode = 200, Message = "Picture retrieved successfully.", Data = result })
                : BadRequest(new ApiResponse<string> { StatusCode = 400, Message = "There is no picture." });
        }

        [Authorize]
        [HttpPut("Update-freelancer-picture")]
        public async Task<IActionResult> UpdateUserPictureAsync(IFormFile? file)
        {
            var result = await _freelancerService.UpdateFreelancerPictureAsync(file);
            return result
                ? Ok(new ApiResponse<string> { StatusCode = 200, Message = "Picture has been added successfully." })
                : BadRequest(new ApiResponse<string> { StatusCode = 400, Message = "Failed to add picture." });
        }

        [Authorize]
        [HttpDelete("delete-freelancer-picture")]
        public async Task<IActionResult> DeleteUserPictureAsync()
        {
            var result = await _freelancerService.DeleteFreelancerPictureAsync();
            return result
                ? Ok(new ApiResponse<string> { StatusCode = 200, Message = "Picture has been deleted successfully." })
                : BadRequest(new ApiResponse<string> { StatusCode = 400, Message = "Failed to delete picture." });
        }
        #endregion

    }
}

