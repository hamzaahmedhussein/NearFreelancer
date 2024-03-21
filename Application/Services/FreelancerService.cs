using AutoMapper;
using Connect.Application.DTOs;
using Connect.Application.Helpers;
using Connect.Application.Settings;
using Connect.Core.Entities;
using Connect.Core.Interfaces;
using Connect.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Connect.Application.Services
{
    public class FreelancerService : IFreelancerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        public FreelancerService(IUnitOfWork unitOfWork,
            IConfiguration config, IMapper mapper,
            IUserHelpers userHelpers)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        public async Task<bool> AddFreelancerBusiness(AddFreelancerBusinessDto freelancerDto)
        {
            if (freelancerDto == null)
            {
                return false;
            }

            var user = await _userHelpers.GetCurrentUserAsync();

            if (user == null || user.ProfileType == ProfileType.Freelancer)
            {
                return false;
            }

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
           var profile= _unitOfWork.FreelancerBusiness.GetById(id);
                    return _mapper.Map<FreelancerBusinessResult>(profile);
        }



    }
}
