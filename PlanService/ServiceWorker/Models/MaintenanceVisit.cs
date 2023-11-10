using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public class MaintenanceVisit
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string VehicleId { get; set; }

    public string Description { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public VisitType Type { get; set; }
    public string Contact { get; set; }
}

public enum VisitType
{
    Service,
    Repair
}

