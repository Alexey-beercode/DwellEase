using DwellEase.Domain.Entity;
using Microsoft.EntityFrameworkCore.Migrations;
using MongoDB.Bson;
using MongoDB.Driver;
using Migration = MongoDB.Migrations.Migration;

namespace DwellEase.DataManagement.Migrations;

[Migration("20230908120000")]
public class UpdateUsersCollection : Migration
{
    public void Up(IMongoDatabase database)
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("DwellEaseApartmentsDB");
        var userCollection = mongoDatabase.GetCollection<User>("Users");
    }

    public void Down()
    {
        // Определение действий для отката миграции, если это необходимо
        var connectionString = "<Your_Connection_String>";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("<Your_Database_Name>");

        // Удаление коллекции "users" и другие действия отката
        var usersCollection = database.GetCollection<BsonDocument>("users");
        database.DropCollection("users");
    }

    public override Task Run(IMongoDatabase database)
    {
        throw new NotImplementedException();
    }
}