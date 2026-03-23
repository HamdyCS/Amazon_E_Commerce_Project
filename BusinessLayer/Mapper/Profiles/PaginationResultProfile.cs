using AutoMapper;
using DataAccessLayer.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dtos;

namespace BusinessLayer.Mapper.Profiles
{
    public class PaginationResultProfile : Profile
    {
        public PaginationResultProfile() {
          CreateMap(typeof(PaginationResult<>), typeof(PaginationResultDto<>))
                .ForMember("Data"
                , opt => opt.Ignore());

            CreateMap(typeof(PaginationResultDto<>), typeof(PaginationResult<>))
               .ForMember("Data"
               , opt => opt.Ignore());
        }
    }
}
