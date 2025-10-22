using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class CreateProductyProfile : Profile
    {
        public CreateProductyProfile()
        {
            CreateMap<CreateProductDto, Product>().ForMember(e => e.Id,
                otp => otp.Ignore());


            CreateMap<Product, CreateProductDto>();
        }
    }
}
