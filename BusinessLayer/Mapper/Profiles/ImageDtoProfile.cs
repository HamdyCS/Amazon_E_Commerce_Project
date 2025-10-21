using AutoMapper;
using BusinessLayer.Dtos;

namespace BusinessLayer.Mapper.Profiles
{
    public class ImageDtoProfile : Profile
    {

        public ImageDtoProfile()
        {
            CreateMap<ImageDto, ProductCategoryImageDto>().ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Url));
            CreateMap<ProductCategoryImageDto, ImageDto>().ForMember(x => x.Url, opt => opt.MapFrom(x => x.ImageUrl));

        }
    }
}
