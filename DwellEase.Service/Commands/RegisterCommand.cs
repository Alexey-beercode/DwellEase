using DwellEase.Domain.Models.Identity;
using MediatR;

namespace DwellEase.Service.Commands;

public class RegisterCommand:IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}