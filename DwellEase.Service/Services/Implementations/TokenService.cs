using System.IdentityModel.Tokens.Jwt;
using DwellEase.Domain.Entity;
using DwellEase.Service.Extensions;
using DwellEase.Service.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DwellEase.Service.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string CreateToken(User user)
    {
        JwtSecurityToken token = user
            .CreateClaims(new List<Role> { user.Role })
            .CreateJwtToken(_configuration);
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}