using DwellEase.DataManagement.Repositories.Implementations;
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
        var workFactor = 12; 
        var salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
        if (!userCollection.Find(_ => true).Any())
        {
            var user = new User
            {
                Id= new Guid("7b4200d1-2199-4e04-80b6-bda31c152d9b"),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Role = new Role()
                {
                    Id=new Guid("9b0dc8cd-35a0-4ca6-b820-74052c74417b"),
                    RoleName = "Admin",
                    NormalizedRoleName = "ADMIN"
                },
                PasswordSalt = salt,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("1025556478955466Admin445", salt)
            };
            userCollection.InsertOneAsync(user);
        }
    }
}