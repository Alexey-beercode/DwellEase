using System.Net;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Services.Implementations;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly UserRoleRepository _userRoleRepository;
    private readonly ILogger<UserService> _logger;
    private readonly RoleService _roleService;

    public UserService(UserRepository userRepository, ILogger<UserService> logger, RoleService roleService, UserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _roleService = roleService;
        _userRoleRepository = userRoleRepository;
    }

    private BaseResponse<T> HandleError<T>(string description, HttpStatusCode error)
    {
        var response = new BaseResponse<T>
        {
            StatusCode = error,
            Description = description
        };
        _logger.LogError(description);
        return response;
    }

    private Task<List<User>> CorrectUsersRefreshTokenExpiryTime(List<User> users)
    {
        return Task.FromResult(users.Select(user =>
        {
            user.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime.ToLocalTime();
            return user;
        }).ToList());
    }

    private string HashPassword(string password,string salt)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return (hashedPassword);
    }

    public async Task CreateAsync(User user, string password)
    {
        var workFactor = 12;
        var salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
        user.PasswordSalt = salt;
        user.PasswordHash = HashPassword(password, salt);
        await _userRepository.Create(user);
    }


    public async Task<BaseResponse<bool>> AddToRoleAsync(User user, string role)
    {
        if (!(role == "Creator" || role == "Resident"))
        {
            return HandleError<bool>("Role are not contains right role type", HttpStatusCode.BadRequest);
        }

        var roles = (await _roleService.GetAllAsync()).Data;
        var newRole = roles.FirstOrDefault(a => a.RoleName == role);
        await _userRoleRepository.Create(new UserRole() { UserId = user.Id, RoleId = newRole.Id });
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var user = await await _userRepository.GetById(id);
        if (user == null)
        {
            return HandleError<bool>($"User with id: {id} not found", HttpStatusCode.NoContent);
        }

        await _userRepository.Delete(id);
        _logger.LogInformation($"Successfully delete user with id: {id}");
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<User>> GetByIdAsync(Guid id)
    {
        var user = await await _userRepository.GetById(id);
        if (user == null)
        {
            return HandleError<User>($"User with id: {id} not found", HttpStatusCode.NoContent);
        }

        user=(await CorrectUsersRefreshTokenExpiryTime(new List<User>() { user }))[0];
        _logger.LogInformation($"Successfully get user with id: {id}");
        return new BaseResponse<User>() { Data = user, StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<User>> FindByNameAsync(string userName)
    {
        var user = (await await _userRepository.GetAll()).FirstOrDefault(a => a.UserName == userName);
        if (user == null)
        {
            return HandleError<User>($"User with username: {userName} not found", HttpStatusCode.NoContent);
        }
        user=(await CorrectUsersRefreshTokenExpiryTime(new List<User>() { user }))[0];
        return new BaseResponse<User>() { Data = user, StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<bool>> UpdateAsync(User user)
    {
        var findUser = await await _userRepository.GetById(user.Id);
        if (findUser == null)
        {
            return HandleError<bool>($"User with id: {user.Id} not found", HttpStatusCode.NoContent);
        }

        await _userRepository.Update(user);
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }
    
    public async Task<BaseResponse<bool>> UpdateCridentialsAsync(UpdateUserRequest user)
        {
            var findUser = await await _userRepository.GetById(Guid.Parse(user.UserId));
            if (findUser == null)
            {
                return HandleError<bool>($"User with id: {Guid.Parse(user.UserId)} not found", HttpStatusCode.NoContent);
            }
    
            await _userRepository.UpdateCridentials(user,HashPassword(user.Password,findUser.PasswordSalt));
            return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
        }
    

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        var findUser =await await _userRepository.GetById(user.Id);
        if (findUser == null)
        {
            throw new Exception($"User with id: {user.Id} not found");
        }

        return BCrypt.Net.BCrypt.Verify(password, findUser.PasswordHash);
    }
    
    public async Task<BaseResponse<List<User>>> GetAllAsync()
    {
        var users = await await _userRepository.GetAll();
        if (users.Count == 0)
        {
            return HandleError<List<User>>("Users are not found", HttpStatusCode.NoContent);
        }
        users=await CorrectUsersRefreshTokenExpiryTime(users);
        return new BaseResponse<List<User>>() { Data = users, StatusCode = HttpStatusCode.OK };
    }
}