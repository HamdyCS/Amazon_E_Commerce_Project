using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class CreateBrandProfile : Profile
    {
        public CreateBrandProfile()
        {
            CreateMap<CreateBrandDto, BrandDto>().ReverseMap();

            CreateMap<CreateBrandDto, Brand>().ReverseMap();
        }
    }
}
