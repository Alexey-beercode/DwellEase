using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Implementations;

public class RoleRepository:IBaseRepository<Role>
{
    private readonly IMongoCollection<Role> _collection;

    public RoleRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Role>("Roles");
    }

    public Task Create(Role model)
    {
        return _collection.InsertOneAsync(model);
    }

    public Task Delete(Guid id)
    {
        var filter = Builders<Role>.Filter.Eq(a => a.Id, id);
        return _collection.DeleteOneAsync(filter);
    }

    public Task Update(Role model)
    {
        var filter = Builders<Role>.Filter.Eq(a => a.Id, model.Id);
        var update = Builders<Role>.Update
            .Set(a => a.RoleName, model.RoleName)
            .Set(a => a.NormalizedRoleName, model.NormalizedRoleName);
        return _collection.FindOneAndUpdateAsync(filter, update);
    }

    public async Task<Task<List<Role>>> GetAll()
    {
        var filter = Builders<Role>.Filter.Empty;
        var roles = await _collection.FindAsync(filter);
        return roles.ToListAsync();
    }

    public async Task<Task<Role>> GetById(Guid id)
    {
        var filter = Builders<Role>.Filter.Eq(a => a.Id, id);
        return (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
    }
}