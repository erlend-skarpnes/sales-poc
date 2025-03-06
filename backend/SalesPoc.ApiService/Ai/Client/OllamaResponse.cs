using System.Text.Json.Serialization;

namespace SalesToolPoc.ApiService.Ai.Client;

public class OllamaResponse
{
    [JsonPropertyName("message")]
    public OllamaMessage? Message { get; init; }
}