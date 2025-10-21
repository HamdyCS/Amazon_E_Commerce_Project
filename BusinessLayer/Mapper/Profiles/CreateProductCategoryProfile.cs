using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class CreateProductCategoryProfile : Profile
    {
        public CreateProductCategoryProfile()
        {
            CreateMap<CreateProductCategoryDto, ProductCategory>().ForMember(e => e.Id,
                otp => otp.Ignore());


            CreateMap<ProductCategory, ProductCategoryDto>();
        }
    }
}
