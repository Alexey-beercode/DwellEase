using DwellEase.Domain.Entity;

namespace DwellEase.Service.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(User user, List<Role> roles);
}