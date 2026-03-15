using BusinessLayer.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validations
{
    public class UpdateBannerDtoValidition : AbstractValidator<UpdateBannerDto>
    {
        public UpdateBannerDtoValidition()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required")
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1");

            RuleFor(x => x.DisplayOrder).NotEmpty().WithMessage("DisplayOrder is required")
               .GreaterThanOrEqualTo(1).WithMessage("DisplayOrder must be greater than or equal to 1");

            RuleFor(x => x.IsActive).NotNull().WithMessage("IsActive is required");

            RuleFor(x => x.StartDate).NotEmpty().WithMessage("StartDate is required")
              .Must(x => x.Date >= DateTime.UtcNow.Date).WithMessage("StartDate must be greater than or eqaul today");

            RuleFor(x => x.EndDate).NotEmpty().WithMessage("EndDate is required")
                .Must((dto, endDate) => endDate.Date >= dto.StartDate.Date).WithMessage("EndDate must be greater than or equal StartDate ");
        }
    }
}
