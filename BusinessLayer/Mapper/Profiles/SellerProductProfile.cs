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
    public class SellerProductProfile : Profile
    {
        public SellerProductProfile()
        {
            CreateMap<SellerProductDto, SellerProduct>().ForMember(e => e.Id
            , otp => otp.Ignore());

            CreateMap<SellerProduct, SellerProductDto>();
        }
    }
}
