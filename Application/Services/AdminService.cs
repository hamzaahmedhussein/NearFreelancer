using Connect.Application.DTOs;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SystemStatisticsDto> GetLastMonthStatisticsAsync()
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

            var totalCustomers = await _unitOfWork.Customer.CountAsync();
            var newCustomers = await _unitOfWork.Customer.CountAsync(c => c.DOJ >= oneMonthAgo);
            var totalFreelancers = await _unitOfWork.FreelancerBusiness.CountAsync();
            var newFreelancers = await _unitOfWork.FreelancerBusiness.CountAsync(f => f.DOJ >= oneMonthAgo);
            var totalServiceRequests = await _unitOfWork.ServiceRequest.CountAsync();
            var newServiceRequests = await _unitOfWork.ServiceRequest.CountAsync(sr => sr.DateTime >= oneMonthAgo);
            var totalRevenue = await _unitOfWork.ServiceRequest
            .SumAsync(sr => sr.DateTime >= oneMonthAgo ? sr.Price : 0m);

            var acceptedRequests = await _unitOfWork.ServiceRequest
                .CountAsync(sr => sr.Status == RequisStatus.Accepted && sr.DateTime >= oneMonthAgo);

            return new SystemStatisticsDto
            {
                TotalCustomers = totalCustomers,
                NewCustomers = newCustomers,
                TotalFreelancers = totalFreelancers,
                NewFreelancers = newFreelancers,
                TotalServiceRequests = totalServiceRequests,
                NewServiceRequests = newServiceRequests,
                TotalRevenue = totalRevenue,
                AcceptedRequests = acceptedRequests
            };
        }
    }
}
