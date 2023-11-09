using Microsoft.VisualBasic;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRepository
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoClient _client;

    private readonly IMongoDatabase _database;


    public UserRepository(IConfiguration configuration)
    {
        _client = new MongoClient(configuration["connectionString"] ?? "mongodb://localhost:27018");
        _database = _client.GetDatabase(configuration["databaseName"] ?? String.Empty);
        _userCollection = _database.GetCollection<User>(configuration["collectionName"] ?? String.Empty);
    }

    public async Task<List<User>> GetAllUsers()
    {
        var users = await _userCollection.Find(u => true).ToListAsync();
        return users;
    }

    public async Task<User> GetUserById(string userId)
    {
        var user = await _userCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
        return user;
    }

    public async Task InsertUser(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task UpdateUser(string userId, User updatedUser)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
        var update = Builders<User>.Update
            .Set(u => u.Username, updatedUser.Username)
            .Set(u => u.Email, updatedUser.Email);

        await _userCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteUser(string userId)
    {
        await _userCollection.DeleteOneAsync(u => u.Id == userId);
    }
}