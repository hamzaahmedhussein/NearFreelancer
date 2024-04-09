using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController : ControllerBase
    {
       private readonly IFreelancerService _freelancerService;
        public FreelancerController(IFreelancerService freelancerService)
        {  
            _freelancerService = freelancerService;
        }



        [Authorize]
        [HttpPost("add-freelancer-business")]
        public async Task<IActionResult> AddFreelancerBusiness( AddFreelancerBusinessDto freelancerDto)
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
                return BadRequest("Already has Freelancer");
            }
        }

        [HttpGet("freelancer-profile")]
        public async Task<IActionResult> GetFreelancerProfile()
        {
            var result = await _freelancerService.GetFreelancerProfile();
            return result != null ? Ok(result) : NotFound("Freelancer profile not found.");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreelancerById(int id)
        {
            try
            {
                var result = await _freelancerService.GetFreelancerById(id);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound(new { message = "Freelancer not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    

        [Authorize]       
        [HttpPost("add-offered-service")]
        public async Task<IActionResult> AddOfferedService([FromBody] AddOfferedServiceDto serviceDto, IFormFile? file)
        {
            var result = await _freelancerService.AddOfferedService(serviceDto);
            if (result)
            {
                return Ok("Offered service added successfully.");
            }
            return BadRequest("Failed to add offered service.");
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
        public async Task<IActionResult> GetMyRequests()
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

        [HttpPost("accept/{requestId}")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            try
            {
                var result = await _freelancerService.AcceptServiceRequest(requestId);
                if (result)
                    return Ok(new { message = "Request accepted successfully." });
                else
                    return BadRequest(new { message = "Failed to accept request." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refuse/{requestId}")]
        public async Task<IActionResult> RefuseRequest(int requestId)
        {
            try
            {
                var result = await _freelancerService.RefuseServiceRequest(requestId);
                if (result)
                    return Ok(new { message = "Request refused successfully." });
                else
                    return BadRequest(new { message = "Failed to refuse request." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}

