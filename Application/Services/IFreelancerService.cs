using Connect.Application.DTOs;
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
        Task<FreelancerBusinessResult> GetFreelancerById(int id);
        Task<bool> AddOfferedService(AddOfferedServiceDto serviceDto);
        Task<IEnumerable<FreelancerFilterResultDto>> FilterFreelancers(FilterFreelancersDto filterDto);
    }
}
