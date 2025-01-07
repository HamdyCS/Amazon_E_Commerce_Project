using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Help;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>().ForMember(e => e.Id,
                opt => opt.Ignore()).
                ForMember(e => e.DescriptionEn,
                opt => opt.MapFrom(e =>
                Helper.ReturnNullIfEmpty(e.DescriptionEn)))
                .ForMember(e=>e.DescriptionAr,opt=>
                opt.MapFrom(e=>Helper.ReturnNullIfEmpty(e.DescriptionAr)));
               

            CreateMap<Product, ProductDto>();
        }

       

        
    }
}
