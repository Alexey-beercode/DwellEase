using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Entity;
using SharedLibrary.Extensions;
using SharedLibrary.Services.Interfaces;

namespace SharedLibrary.Services.Implementations;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    public string CreateToken(User user, List<Role> roles)
    {
        JwtSecurityToken token = user
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
        var tokenHandler = new JwtSecurityTokenHandler();
        
        return tokenHandler.WriteToken(token);
    }
}