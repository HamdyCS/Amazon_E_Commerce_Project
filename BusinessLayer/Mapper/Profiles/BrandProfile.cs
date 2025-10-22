using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandDto, Brand>().ForMember(e => e.Id, otp => otp.Ignore()).ForMember(e => e.PublicId, opt => opt.MapFrom(x => x.Image.PublicId)).ForMember(e => e.ImageUrl, opt => opt.MapFrom(x => x.Image.Url));


            CreateMap<Brand, BrandDto>().ForMember(brandDto => brandDto.Image, opt => opt.MapFrom(brand => new ImageDto() { PublicId = brand.PublicId, Url = brand.ImageUrl }));

        }
    }
}
