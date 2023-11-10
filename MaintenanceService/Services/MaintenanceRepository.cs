using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

public class MaintenanceRepository
{
    private readonly IMongoCollection<MaintenanceVisit> _maintenanceVisits;

    private readonly IMongoCollection<MaintenanceVisit> _maintenanceCollection;
    private readonly IMongoClient _client;

    private readonly IMongoDatabase _database;

    private readonly ILogger<MaintenanceRepository> _logger;

    public MaintenanceRepository(IConfiguration configuration, ILogger<MaintenanceRepository> logger)
    {
        // _maintenanceVisits = _database.GetCollection<MaintenanceVisit>("maintenanceVisits");
        _client = new MongoClient(configuration["connectionString"] ?? "mongodb://localhost:27018");
        _database = _client.GetDatabase(configuration["databaseName"] ?? String.Empty);
        _maintenanceCollection = _database.GetCollection<MaintenanceVisit>(configuration["maintenanceCollectionName"] ?? String.Empty);
        _logger = logger;
    }


    public async Task<List<MaintenanceVisit>> GetAllMaintenanceVisits()
    {
        return await Task.FromResult(_maintenanceCollection.Find(maintenanceVisit => true).ToList());

    }

    public async Task<MaintenanceVisit> GetMaintenanceVisitById(string id)
    {
        
        return await Task.FromResult(_maintenanceCollection.Find(maintenanceVisit => maintenanceVisit.Id == id).FirstOrDefault());

    }

/* 
    public async Task InsertMaintenanceVisit(MaintenanceVisit maintenanceVisit)
    {
        await _maintenanceCollection.InsertOneAsync(maintenanceVisit);
    }

    public async Task UpdateMaintenanceVisit(string id, MaintenanceVisit updatedMaintenanceVisit)
    {
        await _maintenanceCollection.ReplaceOneAsync(visit => visit.Id == id, updatedMaintenanceVisit);
    }

    public async Task DeleteMaintenanceVisit(string id)
    {
        await _maintenanceCollection.DeleteOneAsync(visit => visit.Id == id);
    }

    */
}