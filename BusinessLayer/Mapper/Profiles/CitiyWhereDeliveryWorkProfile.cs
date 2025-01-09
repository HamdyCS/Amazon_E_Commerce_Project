using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper.Profiles
{
    public class CitiyWhereDeliveryWorkProfile : Profile
    {
        public CitiyWhereDeliveryWorkProfile()
        {
            CreateMap<CityWhereDeliveryWorkDto, CityWhereDeliveryWork>().ForMember(e => e.Id,
                opt => opt.Ignore());

            CreateMap<CityWhereDeliveryWork, CityWhereDeliveryWorkDto>();

            CreateMap<CityDto, CityWhereDeliveryWorkDto>().ForMember(e => e.Id,
                opt => opt.Ignore())
                .ForMember(e=>e.CityNameEn,opt=>
                opt.MapFrom(e=>e.NameEn)).ForMember(e => e.CityNameAr,
                opt =>
                opt.MapFrom(e => e.NameAr));

        }
    }
}
