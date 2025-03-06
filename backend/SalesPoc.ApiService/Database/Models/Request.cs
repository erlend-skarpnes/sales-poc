using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SalesToolPoc.ApiService.Database.Models;

public class Request
{
    public Request()
    {
    }
    
    public Request(string text, string emailId)
    {
        _id = ObjectId.GenerateNewId();
        Hash = GetHash(text);
        EmailId = emailId;
        RawData = text;
    }
    
    [BsonId]
    public ObjectId _id { get; set; }
    
    [BsonElement("emailId")]
    public string EmailId { get; set; }
    
    [BsonElement("hash")]
    public string Hash { get; set; }
    
    [BsonElement("rawData")]
    public string RawData { get; set; }

    private static string GetHash(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        var hash = SHA1.HashData(bytes);
        
        var hashBuilder = new StringBuilder(hash.Length);
        foreach (var t in hash)
        {
            hashBuilder.Append(t.ToString("X2"));
        }
        return hashBuilder.ToString();
    }
}