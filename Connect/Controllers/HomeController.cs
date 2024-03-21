using Connect.Application.DTOs;
using Connect.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost("Home")]
        public ActionResult Home(HomePageFilterDto filterDto)
        {
            var result =  _customerService.GetFilteredProviders(filterDto);
            return Ok(result);
        }
    }
}
