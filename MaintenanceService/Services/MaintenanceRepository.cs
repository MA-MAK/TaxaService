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
        _maintenanceVisits = database.GetCollection<MaintenanceVisit>("MaintenanceVisits");
        _client = new MongoClient(configuration["connectionString"] ?? "mongodb://localhost:27018");
        _database = _client.GetDatabase(configuration["databaseName"] ?? String.Empty);
        _maintenanceCollection = _database.GetCollection<MaintenanceVisit>(configuration["collectionName"] ?? String.Empty);
        _logger = logger;
    }


    public async Task<List<MaintenanceVisit>> GetAllMaintenanceVisits()
    {
        return await maintenanceVisits.Find( => true).ToListAsync();
    }

    public async Task<MaintenanceVisit> GetMaintenanceVisitById(string id)
    {
        return await _maintenanceVisits.Find(visit => visit.Id == id).FirstOrDefaultAsync();
    }

    public async Task InsertMaintenanceVisit(MaintenanceVisit maintenanceVisit)
    {
        await _maintenanceVisits.InsertOneAsync(maintenanceVisit);
    }

    public async Task UpdateMaintenanceVisit(string id, MaintenanceVisit updatedMaintenanceVisit)
    {
        await _maintenanceVisits.ReplaceOneAsync(visit => visit.Id == id, updatedMaintenanceVisit);
    }

    public async Task DeleteMaintenanceVisit(string id)
    {
        await _maintenanceVisits.DeleteOneAsync(visit => visit.Id == id);
    }
}