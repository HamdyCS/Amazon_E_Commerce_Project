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
    public class CityProfile : Profile
    {

        public CityProfile()
        {
            CreateMap<CityDto, City>().ForMember(C => C.NameEn, opt => opt.MapFrom(CD => CD.NameEn)).
                ForMember(C => C.NameAr, opt => opt.MapFrom(CD => CD.NameAr))
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.CreatedBy, opt => opt.Ignore());

            CreateMap<City, CityDto>();
        }
    }
}
