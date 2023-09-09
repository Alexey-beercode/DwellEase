using System.Reflection;
using DwellEase.DataManagement.Migrations;
using MongoDB.Driver;
using MongoDB.Migrations;

namespace DwellEase.DataManagement;

public static class MongoDbMigrationConfig
{
    public static void Configure()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("DwellEaseApartmentsDB");
        IMigrationResolver resolver = new DefaultMigrationResolver();
        var migrationRunner = new MigrationRunner(mongoDatabase, resolver);
        Assembly targetAssembly = typeof(UpdateUsersCollection).Assembly;
        migrationRunner.RunMigrations(targetAssembly);
    }
    
}