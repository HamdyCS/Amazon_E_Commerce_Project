using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class ProductCategoryImageProfile : Profile
    {
        public ProductCategoryImageProfile()
        {
            CreateMap<ProductCategoryImage, ProductCategoryImageDto>();
            CreateMap<ProductCategoryImageDto, ProductCategoryImage>().ForMember(e => e.Id,
                opt => opt.Ignore());

            CreateMap<ImageDto, ProductCategoryImageDto>().ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Url));
            CreateMap<ProductCategoryImageDto, ImageDto>().ForMember(x => x.Url, opt => opt.MapFrom(x => x.ImageUrl));
        }
    }
}
