using AutoMapper;
using BusinessLayer.Dtos;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper
{
    public class PersonProfile : Profile
    {
        public PersonProfile() 
        {
            CreateMap<Person, PersonDto>().ReverseMap();

           
        }
    }
}
