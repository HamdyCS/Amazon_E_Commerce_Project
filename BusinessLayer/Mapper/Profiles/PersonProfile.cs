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
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, Person>().ForMember(PD => PD.Id, opt => opt.Ignore());

            CreateMap<Person, UserDto>().ForMember(u => u.Id, opt => opt.Ignore());
            CreateMap<UserDto, Person>().ForMember(p=>p.Id, opt => opt.Ignore());

        }
    }
}
