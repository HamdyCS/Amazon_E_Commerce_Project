using AutoMapper;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Mapper.Profiles
{
    public class EmailContentProfile : Profile
    {
        public EmailContentProfile() { 
            CreateMap<EmailContentDto,UpdateOrderEmailQueueDto>().ReverseMap();
        }
    }
}
