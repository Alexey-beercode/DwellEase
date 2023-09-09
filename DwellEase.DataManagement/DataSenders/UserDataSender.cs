using DwellEase.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.DataSenders;

public class UserDataSeeder
{
    public static void SeedData()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("DwellEaseApartmentsDB");
        var userCollection = mongoDatabase.GetCollection<User>("Users");
        if (!userCollection.Find(_ => true).Any())
        {
            var user = new User
            {
                Id=Guid.NewGuid(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Role = new Role()
                {
                    RoleName = "Admin",
                    NormalizedRoleName = "ADMIN"
                },
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "169032048414Admin1526653")
            };

            userCollection.InsertOneAsync(user);
        }
    }
}