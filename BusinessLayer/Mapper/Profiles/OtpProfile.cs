using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper.Profiles
{
    public class OtpProfile : Profile
    {
        public OtpProfile()
        {
            CreateMap<OtpDto, Otp>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Otp, OtpDto>();
        }
    }
}
