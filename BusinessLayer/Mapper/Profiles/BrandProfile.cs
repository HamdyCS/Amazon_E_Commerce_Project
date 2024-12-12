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
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandDto, Brand>().ForMember(e => e.Id, otp => otp.Ignore())
                .ForMember(e => e.CreatedBy, otp => otp.Ignore());

            CreateMap<Brand, BrandDto>();

        }
    }
}
