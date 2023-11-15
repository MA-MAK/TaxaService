using Microsoft.VisualBasic;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceWorker.Models;

namespace ServiceWorker
{

    public class PlanRepository
    {
        private readonly IMongoCollection<Plan> _planCollection;
         private readonly IMongoCollection<MaintenanceVisit> _maintenanceCollection;
        private readonly IMongoClient _client;

        private readonly ILogger<PlanRepository> _logger;

        private readonly IMongoDatabase _database;


        public PlanRepository(IConfiguration configuration, ILogger<PlanRepository> logger)
        {
            _logger = logger;
            _client = new MongoClient(configuration["connectionString"] ?? "mongodb://localhost:27018");
            _database = _client.GetDatabase(configuration["databaseName"] ?? String.Empty);
            _logger.LogInformation("Database Name: " + configuration["databaseName"]);
            _planCollection = _database.GetCollection<Plan>(configuration["planCollectionName"] ?? String.Empty);
            _maintenanceCollection = _database.GetCollection<MaintenanceVisit>(configuration["maintenanceCollectionName"] ?? String.Empty);
        }

        public async Task<List<Plan>> GetAllPlans()
        {
            var plans = await _planCollection.Find(u => true).ToListAsync();
            return plans;
        }

       /* public async Task<Plan> GetUserById(string Id)
        {
            var user = await _planCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
            return user;
        }
        */
        public async Task InsertPlan(Plan plan)
        {
            await _planCollection.InsertOneAsync(plan);
        }

         public async Task InsertMaintenanceVisit(MaintenanceVisit maintenanceVisit)
        {
            await _maintenanceCollection.InsertOneAsync(maintenanceVisit);
        }


        /*
        public async Task UpdateUser(string userId, User updatedUser)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update
                .Set(u => u.Username, updatedUser.Username)
                .Set(u => u.Email, updatedUser.Email);

            await _planCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteUser(string userId)
        {
            await _planCollection.DeleteOneAsync(u => u.Id == userId);
        }

        */
    }


}