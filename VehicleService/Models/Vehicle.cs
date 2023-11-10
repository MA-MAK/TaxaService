using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;



public class Vehicle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string? Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public double Kilometers { get; set; }

    public List<ServiceRecord> ServiceHistory { get; set; }
    public List<ImageRecord> ImageHistory { get; set; }
}

public class ServiceRecord
{
    
    public DateTime DateTime { get; set; }
    public string ServiceDescription { get; set; }
    public string Mechanic { get; set; }
  
}

public class ImageRecord
{
    
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string AddedByName { get; set; }

    
}

