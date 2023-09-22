using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Implementations
{
    public class UserRoleRepository
    {
        private readonly IMongoCollection<UserRole> _collection;

        public UserRoleRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UserRole>("UserRole");
        }
        public Task Create(UserRole model)
        {
            return _collection.InsertOneAsync(model);
            
        }

        public Task Delete(Guid userId,Guid roleId)
        {
            var filter = Builders<UserRole>.Filter.Eq(a => a.UserId, userId) & Builders<UserRole>.Filter.Eq(a => a.RoleId, roleId);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<List<UserRole>> GetAll()
        {
            var filter = Builders<UserRole>.Filter.Empty;
            var users = await _collection.FindAsync(filter);
            return await users.ToListAsync();
        }
    
    }
}