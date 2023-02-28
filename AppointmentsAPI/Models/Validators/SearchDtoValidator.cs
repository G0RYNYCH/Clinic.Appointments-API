using AppointmentsAPI.Models.Dto;
using FluentValidation;

namespace AppointmentsAPI.Models.Validators;

public class SearchDtoValidator : AbstractValidator<SearchDto>
{
    public SearchDtoValidator()
    {
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0");

        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");
    }
}
