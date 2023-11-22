using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using FluentValidation;

namespace DwellEase.WebAPI.Validators;

public class UpdateUserRequestValidator: AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(request => request.Email).NotNull().WithMessage("Email is null");
        RuleFor(request => request.Password).NotNull().WithMessage("Password is null");
        RuleFor(request => request.UserName).NotNull().MinimumLength(5).WithMessage("UserName is null or smaller than 5");
        RuleFor(request => request.NewPassword).NotNull().MinimumLength(5)
            .WithMessage("New password is null or smaller than 5");
        RuleFor(request => PhoneNumber.IsPhoneValid(request.PhoneNumber).ToString()).NotEqual("false")
            .WithMessage("PhoneNu,number is not valid.");
    }
}