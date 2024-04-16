using Connect.Application.DTOs;
using Connect.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    public interface IFreelancerService
    {
        Task<bool> AddFreelancerBusiness(AddFreelancerBusinessDto freelancerDto);
        Task<FreelancerBusinessResult> GetFreelancerProfile();
        Task<FreelancerBusinessResult> GetFreelancerById(string id);
        Task<bool> AddOfferedService(AddOfferedServiceDto serviceDto);
        Task<bool> UpdateOfferedService(string id, AddOfferedServiceDto serviceDto);
        Task<bool> UpdateFreelancerBusiness(AddFreelancerBusinessDto freelancerDto);
        Task<IEnumerable<FreelancerFilterResultDto>> FilterFreelancers(FilterFreelancersDto filterDto);
        Task<IEnumerable<GetCustomerRequestsDto>> GetFreelancerRequests();
        Task<bool> AcceptServiceRequest(string requestId);
        Task<bool> RefuseServiceRequest(string requestId);
        Task<bool> DeleteFreelancerBusinessAsync();
    }
}
