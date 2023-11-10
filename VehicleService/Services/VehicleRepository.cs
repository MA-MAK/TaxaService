using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;



public class VehicleRepository
{
    private readonly List<Vehicle> _vehicles = new List<Vehicle>();

    private readonly IMongoCollection<Vehicle> _vehicleCollection;
    private readonly IMongoClient _client;

    private readonly IMongoDatabase _database;

    private readonly ILogger<VehicleRepository> _logger;

    public VehicleRepository(IConfiguration configuration, ILogger<VehicleRepository> logger)
    {
        _client = new MongoClient(configuration["connectionString"] ?? "mongodb://localhost:27018");
        _database = _client.GetDatabase(configuration["databaseName"] ?? String.Empty);
        _vehicleCollection = _database.GetCollection<Vehicle>(configuration["collectionName"] ?? String.Empty);
        _logger = logger;
    }


    public async Task<List<Vehicle>> GetAllVehicles()
    {
        return await Task.FromResult(_vehicleCollection.Find(vehicle => true).ToList());
    }

    public async Task<Vehicle> GetVehicleById(string id)
    {
        return await Task.FromResult(_vehicleCollection.Find(vehicle => vehicle.Id == id).FirstOrDefault());
    }

    public async Task InsertVehicle(Vehicle vehicle)
    {
        bool isMongoLive = _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);

        if (isMongoLive)
        {
            // connected
            _logger.LogInformation("Connected to MongoDb");
        }
        else
        {
            // couldn't connect
            _logger.LogInformation("Couldn't connect to MongoDb");
        }
        _vehicleCollection.InsertOne(vehicle);
        _logger.LogInformation($"Inserting vehicle {vehicle.Id} into database:{_database.DatabaseNamespace.DatabaseName} and collection: {_vehicleCollection.CollectionNamespace.CollectionName}, with connection string: {_client.Settings.Server.ToString()}");
        await Task.CompletedTask;
    }

    public async Task UpdateVehicle(string id, Vehicle updatedVehicle)
    {
        var existingVehicle = _vehicleCollection.Find(vehicle => vehicle.Id == id).FirstOrDefault();

        if (existingVehicle != null)
        {
            existingVehicle.Brand = updatedVehicle.Brand;
            existingVehicle.Model = updatedVehicle.Model;
            existingVehicle.RegistrationNumber = updatedVehicle.RegistrationNumber;
            existingVehicle.Kilometers = updatedVehicle.Kilometers;
        }

        await Task.CompletedTask;
    }

    public async Task DeleteVehicle(string id)
    {
        var existingVehicle = _vehicleCollection.Find(vehicle => vehicle.Id == id).FirstOrDefault();
        if (existingVehicle != null)
        {
            _vehicleCollection.DeleteOne(vehicle => vehicle.Id == id);
        }

        await Task.CompletedTask;
    }
}
