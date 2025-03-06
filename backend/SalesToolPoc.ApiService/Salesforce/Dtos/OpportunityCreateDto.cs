using System.Text.Json.Serialization;

namespace SalesToolPoc.ApiService.Salesforce.Dtos;

public class OpportunityCreateDto
{
    [JsonPropertyName("apiName")]
    public string ApiName => "Opportunity";
    
    [JsonPropertyName("fields")]
    public Dictionary<string, object?> Fields { get; set; }
}