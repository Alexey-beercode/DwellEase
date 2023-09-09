using DwellEase.Domain.Models.Identity;
using MediatR;

namespace DwellEase.Service.Commands;

public class RegisterCommand:IRequest<RegisterRequest>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordConfirm { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Role { get; set; } = null!;
}