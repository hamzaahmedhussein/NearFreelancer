using Connect.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Connect.Application.Services
{
    public interface IFreelancerService
    {
        Task<bool> AddFreelancerBusiness(AddFreelancerBusinessDto freelancerDto);
        Task<bool> UpdateFreelancerBusiness(AddFreelancerBusinessDto freelancerDto);
        Task<bool> DeleteFreelancerBusinessAsync();
        Task<FreelancerProfileResult> GetFreelancerProfile();
        Task<FreelancerProfileResult> GetFreelancerById(string id);
        Task<bool> AddOfferedService(AddOfferedServiceDto serviceDto);
        Task<bool> UpdateOfferedService(string id, AddOfferedServiceDto serviceDto);


        Task<PaginatedResponse<FreelancerResult>> FilterFreelancers(string search, int pageIndex, int pageSize);
        Task<PaginatedResponse<FreelancerServiceRequistResult>> GetFreelancerRequests(int pageIndex, int pageSize);

        Task<bool> AcceptServiceRequest(string requestId);
        Task<bool> RefuseServiceRequest(string requestId);
        Task<PaginatedResponse<OfferedServiceResult>> GetOfferedServicesAsync(string freelancerId, int pageSize, int pageIndex);
        Task<bool> AddFreelancerPictureAsync(IFormFile file);
        Task<bool> UpdateFreelancerPictureAsync(IFormFile? file);
        Task<bool> DeleteFreelancerPictureAsync();
        Task<string> GetFreelancerPictureAsync();
    }
}
