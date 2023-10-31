using System.Data;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using FluentValidation;

namespace DwellEase.WebAPI.Validators;

public class CreateApartmentPageRequestValidator : AbstractValidator<CreateApartmentPageRequest>
{
    public CreateApartmentPageRequestValidator()
    {
        RuleFor(request => request.DailyPrice).GreaterThan(0).WithMessage("Daily price must be greater than 0.");
        RuleFor(request => request.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(request => PhoneNumber.IsPhoneValid(request.PhoneNumber).ToString()).NotEqual("false").WithMessage("PhoneNu,number is not valid.");
        RuleFor(request => request.OwnerId).NotEmpty().WithMessage("Owners Id is null");
        RuleFor(request => request.Area).GreaterThan(0).WithMessage("Area is required.");
        RuleFor(request => request.Building).NotEmpty().WithMessage("Owner ID is required.");
        RuleFor(request => request.Street).NotEmpty().WithMessage("Street is required");
        RuleFor(request => request.City).NotEmpty().WithMessage("City is required.");
        RuleFor(request => request.Rooms).GreaterThan(0).WithMessage("Rooms should be more than 0");
        RuleFor(request => request.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(request => request.HouseNumber).NotEmpty().WithMessage("House number is required.");
        RuleFor(request => request.ApartmentType).NotEmpty().WithMessage("Apartment Type is required.");
        RuleFor(request => request.Building).NotEmpty().WithMessage("Owner ID is required.");
        RuleFor(request => request.Images).Must(images => images != null && images.Count > 0)
            .WithMessage("At least one image is required.");
    }
}