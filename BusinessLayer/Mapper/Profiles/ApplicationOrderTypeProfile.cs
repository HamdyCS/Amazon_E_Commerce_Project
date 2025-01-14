using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Help;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapper.Profiles
{
    public class ApplicationOrderTypeProfile : Profile
    {
        public ApplicationOrderTypeProfile()
        {
            CreateMap<ApplicationOrderTypeDto, ApplicationOrderType>().ForMember(x => x.Id,
                opt => opt.Ignore()).
                ForMember(e => e.Name, opt =>
                opt.Ignore()).
                ForMember(e => e.DescriptionEn, opt =>
                opt.MapFrom(e => Helper.ReturnNullIfEmpty(e.DescriptionEn))).
                ForMember(e => e.DescriptionAr, opt =>
                opt.MapFrom(e => Helper.ReturnNullIfEmpty(e.DescriptionAr)));


            CreateMap<ApplicationOrderType, ApplicationOrderTypeDto>();
        }
    }
}
