using System.Text.Json;
using System.Text.Json.Serialization;

namespace SalesToolPoc.ApiService.Ai.Client;

public class OllamaRequest
{
    [JsonPropertyName("model")]
    public string? Model { get; init; }
    [JsonPropertyName("messages")]
    public IEnumerable<OllamaMessage> Messages { get; init; }
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; }
    [JsonPropertyName("format")]
    public JsonElement? Format { get; init; }
}

public class OllamaMessage
{
    [JsonPropertyName("role")]
    public string Role { get; init; }
    [JsonPropertyName("content")]
    public string Content { get; init; }
}

