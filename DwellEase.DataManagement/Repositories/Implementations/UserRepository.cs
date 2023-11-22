using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Implementations;

public class UserRepository:IBaseRepository<User>
{
    private readonly IMongoCollection<User> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<User>("Users");
    }
    public Task Create(User model)
    {
        return _collection.InsertOneAsync(model);
    }

    public Task Delete(Guid id)
    {
        var filter = Builders<User>.Filter.Eq(a => a.Id, id);
        return _collection.DeleteOneAsync(filter);
    }

    public Task Update(User model)
    {
        var newRefreshTokenTime = model.RefreshTokenExpiryTime.ToLocalTime();
        var filter = Builders<User>.Filter.Eq(a => a.Id, model.Id);
        var update = Builders<User>.Update
            .Set(a => a.PhoneNumber, model.PhoneNumber)
            .Set(a => a.Email, model.Email)
            .Set(a => a.RefreshToken, model.RefreshToken)
            .Set(a => a.PasswordHash, model.PasswordHash)
            .Set(a => a.UserName, model.UserName)
            .Set(a => a.NormalizedUserName, model.NormalizedUserName)
            .Set(a => a.RefreshTokenExpiryTime, newRefreshTokenTime);
        return _collection.FindOneAndUpdateAsync(filter, update);
    }

    public Task UpdateCridentials(UpdateUserRequest model,string hashedPassword)
    {
        var filter = Builders<User>.Filter.Eq<>(a => a.Id, model.UserId);
        var update = Builders<User>.Update
            .Set(a => a.PhoneNumber,new PhoneNumber(model.PhoneNumber))
            .Set(a => a.Email, model.Email)
            .Set(a => a.UserName, model.UserName)
            .Set(a => a.PasswordHash, hashedPassword);
        return _collection.FindOneAndUpdateAsync(filter, update);
    }

    public async Task<Task<List<User>>> GetAll()
    {
        var filter = Builders<User>.Filter.Empty;
        var users = await _collection.FindAsync(filter);
        return users.ToListAsync();
    }

    public async Task<Task<User>> GetById(Guid id)
    {
        var filter = Builders<User>.Filter.Eq(a => a.Id, id);
        return (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
    }
}