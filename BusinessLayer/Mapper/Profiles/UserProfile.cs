using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //from userDto to User
            CreateMap<UserDto, User>().ForMember(user => user.Id, opt => opt.Ignore());
            CreateMap<User, UserDto>();

            //from userDto to Person
            CreateMap<UserDto, PersonDto>().ForMember(personDto => personDto.Id, opt => opt.Ignore());
            CreateMap<PersonDto, UserDto>().ForMember(UD => UD.Id, opt => opt.Ignore());


        }
    }
}
