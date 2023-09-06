using SharedLibrary.Entity;

namespace SharedLibrary.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(User user, List<Role> roles);
}