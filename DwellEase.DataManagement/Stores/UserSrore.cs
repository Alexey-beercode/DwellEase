using DwellEase.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Stores;

public class UserSrore : IUserStore<User>
{
    private readonly IMongoCollection<User> _collection;

    public UserSrore(IMongoDatabase database)
    {
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
        return await _collection.Find(u => u.Id.ToString() == userId).FirstOrDefaultAsync(cancellationToken);
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
}