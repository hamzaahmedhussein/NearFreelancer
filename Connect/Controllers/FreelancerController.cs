using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("add-freelancer-business")]
        public async Task<IActionResult> AddFreelancerBusiness([FromBody] AddFreelancerBusinessDto freelancerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCreated = await _freelancerService.AddFreelancerBusiness(freelancerDto);
            return isCreated ? Ok("Freelancer created successfully.") : BadRequest("Failed to create freelancer.");
        }

        [HttpGet("freelancer-profile")]
        public async Task<IActionResult> GetFreelancerProfile()
        {
            var result = await _freelancerService.GetFreelancerProfile();
            return result != null ? Ok(result) : NotFound("Freelancer profile not found.");
        }
    }
}
