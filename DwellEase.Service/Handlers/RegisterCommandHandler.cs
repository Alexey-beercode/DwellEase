using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoFramework.Linq;

namespace DwellEase.Service.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterRequest>
    {
        private readonly UserManager<User> _userManager;

        public RegisterCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
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
            
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ArgumentException("User are not created");
            }

            var findUser = await _userManager.FindByEmailAsync(request.Email);
            if (findUser == null)
            {
                throw new Exception($"User {request.Email} not found");
            }


            if (findUser == null)
            {
                throw new Exception($"User {request.Email} not found");
            }
            
            var result1= await _userManager.AddToRoleAsync(findUser,request.Role);

            return new RegisterRequest
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}