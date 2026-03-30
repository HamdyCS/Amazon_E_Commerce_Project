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
            //AddSellerProductToShoppingCartDto to SellerProductInShoppingCart
            CreateMap<AddSellerProductToShoppingCartDto, SellerProductInShoppingCart>().ForMember(e => e.Id,
                opt => opt.Ignore()).ForMember
                (e => e.ShoppingCartId, opt =>
                opt.Ignore());

            CreateMap<SellerProductInShoppingCart, AddSellerProductToShoppingCartDto>();

            
            //SellerProductInShoppingCart to SellerProductInShoppingCartDto
            CreateMap<SellerProductInShoppingCart, SellerProductInShoppingCartDto>()
                .ForMember(dest => dest.ProductId,
                opt =>
                opt.MapFrom(src => src.SellerProduct != null ?
                src.SellerProduct.ProductId : 0))
                .

                ForMember(dest => dest.ProductNameEn,
                opt => opt.MapFrom(src =>
                     src.SellerProduct != null &&
                     src.SellerProduct.Product != null
                            ? src.SellerProduct.Product.NameEn ?? string.Empty
                            : string.Empty))

                .ForMember(dest => dest.ProductNameAr, opt =>
                opt.MapFrom(src => src.SellerProduct != null
                && src.SellerProduct.Product != null ? src.SellerProduct.Product.NameAr : string.Empty))

                .ForMember(dest => dest.ProductImageUrl, opt =>
                opt.MapFrom(src => src.SellerProduct != null &&
                src.SellerProduct.Product != null && src.SellerProduct.Product.ProductImages.FirstOrDefault() != null ?
                src.SellerProduct.Product.ProductImages.Select(i => i.ImageUrl).FirstOrDefault()
                ?? string.Empty
                : string.Empty));



            CreateMap<SellerProductInShoppingCartDto, SellerProductInShoppingCart>();
        }
    }
}
