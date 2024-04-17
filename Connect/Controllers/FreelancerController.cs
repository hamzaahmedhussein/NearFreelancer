using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController : ControllerBase
    {
        private readonly IFreelancerService _freelancerService;

        public FreelancerController(IFreelancerService freelancerService)
        {
            _freelancerService = freelancerService ?? throw new ArgumentNullException(nameof(freelancerService));
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreelancerById(string id)
        {
            try
            {
                var result = await _freelancerService.GetFreelancerById(id);
                return result != null ? Ok(result) : NotFound(new { message = "Freelancer not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
        public async Task<IActionResult> GetFreelancerRequests()
        {
            try
            {
                var requests = await _freelancerService.GetFreelancerRequests();
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
    }
}
