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
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentDto, Payment>().ForMember(x => x.Id, opt =>
            opt.Ignore()).ForMember(x=>x.TotalPrice,opt=>
            opt.Ignore());


            CreateMap<Payment, PaymentDto>();
        }
    }
}
