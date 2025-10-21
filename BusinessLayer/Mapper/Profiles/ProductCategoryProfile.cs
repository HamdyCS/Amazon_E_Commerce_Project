using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class ProductCategoryProfile : Profile
    {
        public ProductCategoryProfile()
        {
            CreateMap<ProductCategoryDto, ProductCategory>().ForMember(e => e.Id,
                otp => otp.Ignore());


            CreateMap<ProductCategory, ProductCategoryDto>();

            CreateMap<CreateProductCategoryDto, ProductCategory>().ForMember(e => e.Id,
              otp => otp.Ignore());

            CreateMap<ProductCategory, CreateProductCategoryDto>();

        }
    }
}
