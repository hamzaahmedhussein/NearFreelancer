using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Authorization;
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
    }
}
