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
    public class SellerProductInShoppingCartProfile : Profile
    {
        public SellerProductInShoppingCartProfile() 
        {
            CreateMap<SellerProductInShoppingCartDto, SellerProductInShoppingCart>().ForMember(e => e.Id,
                opt => opt.Ignore()).ForMember
                (e=>e.ShoppingCartId,opt=>
                opt.Ignore());

            CreateMap<SellerProductInShoppingCart, SellerProductInShoppingCartDto>();

        }
    }
}
