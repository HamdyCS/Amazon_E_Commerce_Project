using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dtos;
using FluentValidation;

namespace BusinessLayer.Validitions
{

    //عشان يشتغل مع list of CreateBannerDto
    public class CreateBannerDtoListValidator : AbstractValidator<List<CreateBannerDto>>
    {
       
        public CreateBannerDtoListValidator()
        {
           RuleForEach(x=>x).SetValidator(new CreateBannerDtoValidator());
        }
    }

    public class CreateBannerDtoValidator : AbstractValidator<CreateBannerDto>
    {
        public CreateBannerDtoValidator()
        {

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
            RuleFor(x => x.Link).NotEmpty().WithMessage("Link is required");

            RuleFor(x => x.StartDate).NotEmpty().WithMessage("StartDate is required")
                .Must(x=> x.Date >= DateTime.UtcNow.Date).WithMessage("StartDate must be greater than or eqaul today");

            RuleFor(x => x.EndDate).NotEmpty().WithMessage("EndDate is required")
                .Must((dto,endDate)=> endDate.Date >= dto.StartDate.Date).WithMessage("EndDate must be greater than or equal StartDate ");
        }
    }
}
