using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SalesToolPoc.ApiService.Database.Models;

public class RequestSummary
{
    [BsonId]
    public ObjectId _id { get; set; }
    
    [BsonElement("hash")]
    public string Hash { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("summary")]
    public string Summary { get; set; }
    
    [BsonElement("role")]
    public string Role { get; set; }
    
    [BsonElement("customer")]
    public string Customer { get; set; }
    
    [BsonElement("deadline")]
    public DateTime? Deadline { get; set; }
}