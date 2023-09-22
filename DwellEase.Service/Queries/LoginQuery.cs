using DwellEase.Domain.Models.Identity;
using MediatR;

namespace DwellEase.Service.Queries;

public class LoginQuery:IRequest<LoginResponse>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}