using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Help;
using DataAccessLayer.Entities;

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
                .ForMember(e => e.DescriptionAr, opt =>
                opt.MapFrom(e => Helper.ReturnNullIfEmpty(e.DescriptionAr)));


            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>().ForMember(e => e.Id,
             otp => otp.Ignore());

            CreateMap<Product, CreateProductDto>();

            CreateMap<Product, CacheProductDto>().ReverseMap();
        }

    }
}
