using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Stores;

public class RoleStore : IRoleStore<Role>
{
    private readonly IMongoCollection<Role> _collection;

    public RoleStore(IMongoDatabase database)
    {
        _collection = database.GetCollection<Role>("Roles");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(role, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var filter = Builders<Role>.Filter.Eq(a => a.Id, role.Id);
        var update = Builders<Role>.Update
            .Set(a => a.RoleName, role.RoleName)
            .Set(a => a.NormalizedRoleName, role.NormalizedRoleName);
        await _collection.FindOneAndUpdateAsync(filter, update);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        var filter = Builders<Role>.Filter.Eq(a => a.Id, role.Id);
        await _collection.DeleteOneAsync(filter, cancellationToken);
        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.RoleName)!;
    }

    public async Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken)
    {
        role.RoleName = roleName!;
        await _collection.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
    }

    public async Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        return role.NormalizedRoleName;
    }

    public async Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken)
    {
        role.NormalizedRoleName = normalizedName!;
        await _collection.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
    }

    public async Task<Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        return await _collection.Find(r => r.Id == new Guid(roleId)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        return await _collection.Find(r => r.NormalizedRoleName == normalizedRoleName)
            .FirstOrDefaultAsync(cancellationToken);
    }
}