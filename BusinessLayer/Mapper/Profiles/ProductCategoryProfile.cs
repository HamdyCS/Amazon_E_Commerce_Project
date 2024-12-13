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
    public class ProductCategoryProfile : Profile
    {
        public ProductCategoryProfile()
        {
            CreateMap<ProductCategoryDto, ProductCategory>().ForMember(e => e.Id,
                otp => otp.Ignore());
               

            CreateMap<ProductCategory, ProductCategoryDto>();
        }
    }
}
