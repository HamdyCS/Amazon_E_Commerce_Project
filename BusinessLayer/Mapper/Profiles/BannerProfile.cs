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
    public class BannerProfile : Profile
    {
        public BannerProfile()
        {
            //CreateBannerDto
            CreateMap<CreateBannerDto, Banner>().ReverseMap();
            CreateMap<CreateBannerDto,BannerDto>().ReverseMap();

            //UpdateBannerDto
            CreateMap<UpdateBannerDto, Banner>().ReverseMap();

            //BannerDto
            CreateMap<BannerDto, Banner>().
                ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Banner, BannerDto > ();

        }
    }
}
