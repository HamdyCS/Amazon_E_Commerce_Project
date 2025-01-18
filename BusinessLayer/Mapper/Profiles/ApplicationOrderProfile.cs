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
    public class ApplicationOrderProfile : Profile
    {
        public ApplicationOrderProfile()
        {
            CreateMap<ApplicationOrderDto, ApplicationOrder>().ForMember(x => x.Id, opt =>
            opt.Ignore());

            CreateMap<ApplicationOrder, ApplicationOrderDto>();

            CreateMap<ApplicationOrder, ApplicationOrder>().ForMember(x=>x.Id,opt=>
            opt.Ignore()).ForMember(x=>x.CreatedAt,opt=>
            opt.Ignore()).ForMember(x=>x.ApplicationOrderTypeId,opt=>
            opt.Ignore());
        }
    }
}
