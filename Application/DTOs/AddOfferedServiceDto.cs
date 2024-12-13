using Microsoft.AspNetCore.Http;

namespace Connect.Application.DTOs
{
    public class AddOfferedServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsAvailable { get; set; }
    }
}
