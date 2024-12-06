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
    public class UserAddressProfile : Profile
    {
        public UserAddressProfile()
        {
            CreateMap<UserAddress, UserAddressDto>();

            CreateMap<UserAddressDto, UserAddress>().ForMember(x => x.Id, opt =>
            opt.Ignore());
        }
    }
}
