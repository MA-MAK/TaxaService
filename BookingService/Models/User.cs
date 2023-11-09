using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
[BsonId]
[BsonRepresentation(BsonType.ObjectId)]
public string? Id { get; set; }

[BsonElement("Name")]
public string Username { get; set; }
public string Email { get; set; }
public string Password { get; set; }

}