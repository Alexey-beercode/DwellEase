using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;

namespace DwellEase.Service.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterRequest>
    {
        private readonly UserService _userSrevice;

        public RegisterCommandHandler(UserService userSrevice)
        {
            _userSrevice = userSrevice;
        }

        public async Task<RegisterRequest> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                Role = new Role(){RoleName = request.Role}
            };
            
            await _userSrevice.CreateAsync(user, request.Password);
            

            var response = await _userSrevice.FindByEmailAsync(request.Email);
            if (response.StatusCode!=HttpStatusCode.OK)
            {
                throw new Exception($"User {request.Email} not found");
            }
            
            await _userSrevice.AddToRoleAsync(response.Data,request.Role);

            return new RegisterRequest
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}