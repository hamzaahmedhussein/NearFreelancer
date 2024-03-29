using AutoMapper;
using Connect.Application.DTOs;
using Connect.Application.Helpers;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Connect.Application.Services
{
    public class FreelancerService : IFreelancerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly UserManager<Customer> _userManager;

        public FreelancerService(IUnitOfWork unitOfWork,
            IConfiguration config, IMapper mapper,
            IUserHelpers userHelpers, UserManager<Customer> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _userManager = userManager;
        }
        public async Task<bool> AddFreelancerBusiness(AddFreelancerBusinessDto freelancerDto )
        {
            if (freelancerDto == null)
                return false;

            var user = await _userHelpers.GetCurrentUserAsync();

            if (user == null)
                throw new Exception("User not found.");

            if (await _userManager.IsInRoleAsync(user, "Freelancer"))
                throw new Exception("User already has a freelancer profile.");

            _userHelpers.ChangeUserTypeAsync(2, user);

            var freelancer = _mapper.Map<Freelancer>(freelancerDto);
            freelancer.Owner = user;

            user.Freelancer = freelancer;

            _unitOfWork.Save();

            return true;
        }



        public async Task<FreelancerBusinessResult> GetFreelancerProfile()
        {

            var user = await _userHelpers.GetCurrentUserAsync();

            if (user != null)
            {
                var profile = user.Freelancer;
                if (profile != null)
                {
                    return _mapper.Map<FreelancerBusinessResult>(profile);
                }
            }

            return null;
        }

        public async Task<FreelancerBusinessResult> GetFreelancerById(int id)
        {
            var profile = _unitOfWork.FreelancerBusiness.GetById(id);
            return _mapper.Map<FreelancerBusinessResult>(profile);
        }

        public async Task<bool> AddOfferedService(AddOfferedServiceDto serviceDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                return false;

            var offeredService = new OfferedService
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                Price = serviceDto.Price,
                Image = serviceDto.Image,
                BackgroundImage = serviceDto.BackgroundImage,
                IsAvailable = serviceDto.IsAvailable,
                DOJ = DateTime.Now
            };


            if (currentUser.Freelancer != null)
            {
                currentUser.Freelancer.OfferedServicesList.Add(offeredService);
                _unitOfWork.Save();
                return true;
            }
            return false;

        }
    }
}
