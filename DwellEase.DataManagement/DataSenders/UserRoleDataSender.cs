using DwellEase.Domain.Entity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.DataSenders;

public class UserRoleDataSender
{
    public static void SeedData()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("DwellEase");
        var userCollection = mongoDatabase.GetCollection<UserRole>("UserRole");
        if (!userCollection.Find(_ => true).Any())
        {
            var userRole = new UserRole()
            {
                UserId = new Guid("7b4200d1-2199-4e04-80b6-bda31c152d9b"),
                RoleId = new Guid("9b0dc8cd-35a0-4ca6-b820-74052c74417b")
            };
            userCollection.InsertOneAsync(userRole);
        }
    }
}