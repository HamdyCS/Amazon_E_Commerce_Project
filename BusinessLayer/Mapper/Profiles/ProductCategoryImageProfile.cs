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
    public class ProductCategoryImageProfile : Profile
    {
        public ProductCategoryImageProfile()
        {
            CreateMap<ProductCategoryImage, ProductCategoryImageDto>();
            CreateMap<ProductCategoryImageDto, ProductCategoryImage>().ForMember(e=>e.Id,
                opt=>opt.Ignore());
        }
    }
}
