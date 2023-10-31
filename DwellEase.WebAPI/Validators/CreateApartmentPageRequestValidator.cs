using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using FluentValidation;

namespace DwellEase.WebAPI.Validators;

public class CreateApartmentPageRequestValidator : AbstractValidator<CreateApartmentPageRequest>
{
    public CreateApartmentPageRequestValidator()
    {
        RuleFor(request => request.DaylyPrice).GreaterThan(0).WithMessage("Daily price must be greater than 0.");
        RuleFor(request => request.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(request => PhoneNumber.IsPhoneValid(request.PhoneNumber).ToString()).NotEqual("false").WithMessage("PhoneNu,number is not valid.");
        RuleFor(request => request.OwnerId).NotEmpty().WithMessage("Owners Id is null");
        RuleFor(request => request.Apartment).NotEmpty().WithMessage("Phone number is required.");
        RuleFor(request => request.OwnerId).NotEmpty().WithMessage("Owner ID is required.");
        RuleFor(request => request.Images).Must(images => images != null && images.Count > 0)
            .WithMessage("At least one image is required.");
    }
}