using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductImageDto, ProductImage>().ForMember(e => e.Id,
                opt => opt.Ignore());

            CreateMap<ImageDto, ProductImageDto>().ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Url));
            CreateMap<ProductImageDto, ImageDto>().ForMember(x => x.Url, opt => opt.MapFrom(x => x.ImageUrl));
        }
    }
}
