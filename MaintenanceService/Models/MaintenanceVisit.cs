using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class MaintenanceVisit
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string VehicleId { get; set; }

    public string Description { get; set; }
    public VisitType Type { get; set; }
    public string Contact { get; set; }
}

public enum VisitType
{
    Service,
    Repair
}