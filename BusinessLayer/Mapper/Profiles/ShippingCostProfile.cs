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
    public class ShippingCostProfile : Profile
    {
        public ShippingCostProfile() 
        {
            CreateMap<ShippingCostDto, ShippingCost>()
                .ForMember(e=>e.Id,otp=>
                otp.Ignore()).ForMember(e=>e.CreatedAt,opt=>
                opt.Ignore()).ForMember(e => e.CreatedBy, opt =>
                opt.Ignore());

            CreateMap<ShippingCost, ShippingCostDto>();
        }
    }
}
