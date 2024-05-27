using Connect.Application.DTOs;
using Connect.Application.Services;
using Connect.Core.Models;
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
                return BadRequest(ModelState);

            try
            {
                var isCreated = await _freelancerService.AddFreelancerBusiness(freelancerDto);
                return isCreated ? Ok("Freelancer created successfully.") : BadRequest("Failed to create freelancer.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete-freelancer-business")]
        public async Task<IActionResult> DeleteFreelancerBusiness()
        {
            try
            {
                var isDeleted = await _freelancerService.DeleteFreelancerBusinessAsync();
                return isDeleted ? Ok("Freelancer Deleted successfully.") : BadRequest("Failed to Delete freelancer.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("update-freelancer-business")]
        public async Task<IActionResult> UpdateFreelancerBusiness([FromBody] AddFreelancerBusinessDto freelancerDto)
        {
                var result = await _freelancerService.UpdateFreelancerBusiness(freelancerDto);
                return result ? Ok("Freelancer updated successfully.") : BadRequest("Failed to update freelancer.");
        }


        [HttpGet("freelancer-profile")]
        public async Task<IActionResult> GetFreelancerProfile()
        {
            var result = await _freelancerService.GetFreelancerProfile();
            return result != null ? Ok(result) : NotFound("Freelancer profile not found.");
        }

        [HttpGet("get-freelancer-by-id/{id}")]
        public async Task<IActionResult> GetFreelancerById(string id)
        {
            try
            {
                _logger.LogInformation("API request: Getting freelancer profile for ID: {FreelancerId} with offered services ", id);

                var result = await _freelancerService.GetFreelancerById(id);

                if (result == null)
                {
                    _logger.LogWarning("Freelancer with ID: {FreelancerId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API error: Error retrieving freelancer profile for ID: {FreelancerId}", id);

                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
        [Authorize]
        [HttpPost("add-offered-service")]
        public async Task<IActionResult> AddOfferedService( AddOfferedServiceDto serviceDto)
        {
            try
            {
                var result = await _freelancerService.AddOfferedService(serviceDto);
                return result ? Ok("Offered service added successfully.") : BadRequest("Failed to add offered service.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("update-offered-service")]
        public async Task<IActionResult> UpdateOfferedService(string id, AddOfferedServiceDto serviceDto)
        {
            try
            {
                var result = await _freelancerService.UpdateOfferedService(id,serviceDto);
                return result ? Ok("Offered service Updated successfully.") : BadRequest("Failed to Upfate offered service.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("filter-freelancers")]
        public async Task<IActionResult> FilterFreelancers([FromBody] FilterFreelancersDto filterDto)
        {
            try
            {
                var result = await _freelancerService.FilterFreelancers(filterDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-freelancer-requests")]
        public async Task<IActionResult> GetFreelancerRequests( int pageIndex, int pageSize = 4)
        {
            try
            {
                var requests = await _freelancerService.GetFreelancerRequests( pageIndex, pageSize); 
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("accept/{requestId}")]
        public async Task<IActionResult> AcceptRequest(string requestId)
        {
            try
            {
                var result = await _freelancerService.AcceptServiceRequest(requestId);
                return result ? Ok(new { message = "Request accepted successfully." }) : BadRequest(new { message = "Failed to accept request." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("refuse/{requestId}")]
        public async Task<IActionResult> RefuseRequest(string requestId)
        {
            try
            {
                var result = await _freelancerService.RefuseServiceRequest(requestId);
                return result ? Ok(new { message = "Request refused successfully." }) : BadRequest(new { message = "Failed to refuse request." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
                
          
        }
        [Authorize]
        [HttpGet("get-offered-services/{freelancerId}")]
        public async Task<IActionResult> GetOfferedService(string freelancerId, int pageIndex, int pageSize=4)
        {
            if (string.IsNullOrWhiteSpace(freelancerId))
            {
                return BadRequest("Freelancer ID must be provided.");
            }

            var offeredServiceResults = await _freelancerService.GetOfferedServicesAsync(freelancerId, pageIndex, pageSize);
            return Ok(offeredServiceResults);
        }


        #region file handling
        [Authorize]
        [HttpGet("get-freelancer-picture")]
        public async Task<IActionResult> GetUserPictureAsync()
        {
            var result = await _freelancerService.GetFreelancerPictureAsync();
            return result != string.Empty ? Ok(result) : BadRequest("there is not picture.");
        }
        [Authorize]
        [HttpPost("add-freelancer-picture")]
        public async Task<IActionResult> AddUserPictureAsync(IFormFile? file)
        {
            var result = await _freelancerService.AddFreelancerPictureAsync(file);
            return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        }
        [Authorize]
        [HttpPut("Update-freelancer-picture")]
        public async Task<IActionResult> UpdateUserPictureAsync(IFormFile? file)
        {
            var result = await _freelancerService.UpdateFreelancerPictureAsync(file);
            return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        }
        [Authorize]
        [HttpDelete("delete-freelancer-picture")]
        public async Task<IActionResult> DeleteUserPictureAsync()
        {
            var result = await _freelancerService.DeleteFreelancerPictureAsync();
            return result ? Ok("picture has been deleted successfully.") : BadRequest("failed to delete picture");
        }
        #endregion
    }
}
