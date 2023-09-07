using DwellEase.Domain.Models.Identity;
using MediatR;

namespace DwellEase.Service.Queries;

public class LoginQuery:IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }   
}