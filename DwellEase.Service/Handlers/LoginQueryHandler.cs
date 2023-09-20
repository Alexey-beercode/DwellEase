using System.Net;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Extensions;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace DwellEase.Service.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public LoginQueryHandler(IConfiguration configuration, ITokenService tokenService, UserService userService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var response = await _userService.FindByNameAsync(request.UserName);

            if (response.StatusCode!=HttpStatusCode.OK)
            {
                throw new AggregateException("User are not found");
            }

            var isPasswordValid = (await _userService.CheckPasswordAsync(response.Data, request.Password)).Data;

            if (!isPasswordValid)
            {
                throw new ApplicationException("Bad credentials");
            }

            var user = response.Data;

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var accessToken = _tokenService.CreateToken(user);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(_configuration.GetSection("Jwt:TokenValidityInHours").Get<int>());
            await _userService.UpdateAsync(user);
            return new AuthResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = user.RefreshToken
            };
        }
    }
}
