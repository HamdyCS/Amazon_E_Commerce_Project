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
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCartDto, ShoppingCart>().ForMember(x => x.Id,
                opt => opt.Ignore());

            CreateMap<ShoppingCart, ShoppingCartDto>();
        }
    }
}
