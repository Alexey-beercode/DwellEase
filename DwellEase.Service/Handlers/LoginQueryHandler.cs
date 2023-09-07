using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Extensions;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DwellEase.Service.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;

        public LoginQueryHandler(UserManager<User> userManager, IConfiguration configuration, RoleManager<Role> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var managedUser = await _userManager.FindByEmailAsync(request.Email);

            if (managedUser == null)
            {
                throw new ApplicationException("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);

            if (!isPasswordValid)
            {
                throw new ApplicationException("Bad credentials");
            }

            var user = _userManager.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var roleIds = await _roleManager.Roles.Where(r => r.UserId == user.Id).Select(x => x.Id).ToListAsync();
            var roles = _roleManager.Roles.Where(x => roleIds.Contains(x.Id)).ToList();

            var accessToken = _tokenService.CreateToken(user, roles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

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
