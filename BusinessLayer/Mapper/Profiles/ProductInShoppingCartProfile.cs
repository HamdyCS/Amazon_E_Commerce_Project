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
    public class ProductInShoppingCartProfile : Profile
    {
        public ProductInShoppingCartProfile() 
        {
            CreateMap<ProductInShoppingCartDto, ProductInShoppingCart>().ForMember(e => e.Id,
                opt => opt.Ignore()).ForMember
                (e=>e.ShoppingCartId,opt=>
                opt.Ignore());

            CreateMap<ProductInShoppingCart, ProductInShoppingCartDto>();

        }
    }
}
