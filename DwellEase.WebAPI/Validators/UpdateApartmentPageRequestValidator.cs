using System.Data;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using FluentValidation;

namespace DwellEase.WebAPI.Validators;

public class UpdateApartmentPageRequestValidator: AbstractValidator<UpdateApartmentPageRequest>
{
    public UpdateApartmentPageRequestValidator()
    {
        RuleFor(request => request.Price).GreaterThan(0).WithMessage("Price is not valid");
        RuleFor(request => request.DailyPrice).GreaterThan(0).WithMessage("DailyPrice is not valid");
        RuleFor(request => request.Title).NotNull().WithMessage("Title is not valid");
        RuleFor(request => PhoneNumber.IsPhoneValid(request.PhoneNumber).ToString()).NotEqual("false")
            .WithMessage("Phonenumber is not valid.");
    }
}