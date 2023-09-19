using DwellEase.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Stores;

public class UserSrore : IUserEmailStore<User>, IUserPasswordStore<User>, IQueryableUserStore<User>,
    IUserRoleStore<User>
{
    private readonly IMongoCollection<User> _collection;
    private readonly ILogger<UserSrore> _logger;
    public IQueryable<User> Users { get; }

    public UserSrore(IMongoDatabase database, ILogger<UserSrore> logger)
    {
        _logger = logger;
        _collection = database.GetCollection<User>("Users");
    }


    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(user, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(a => a.Id, user.Id);
        await _collection.ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(a => a.Id, user.Id);
        await _collection.DeleteOneAsync(filter, cancellationToken);
        return IdentityResult.Success;
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
    }

    public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return user.NormalizedUserName;
    }

    public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
    }

    public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _collection.Find(u => u.Id == new Guid(userId)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _collection.Find(u => u.NormalizedUserName == normalizedUserName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return user.PasswordHash;
    }

    public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return !string.IsNullOrWhiteSpace(user.PasswordHash);
    }

    public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await _collection.Find(u => u.NormalizedEmail == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        var update = Builders<User>.Update.Set(u => u.Role.RoleName, roleName);
        await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        var update = Builders<User>.Update.Set(u => u.Role.RoleName, "");
        await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }


    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
    {
        var roles = new List<string> { user.Role.RoleName };
        return roles;
    }

    public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        return user.Role.RoleName == roleName;
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _collection.Find(u => u.Role.RoleName == roleName).ToListAsync(cancellationToken);
    }
}