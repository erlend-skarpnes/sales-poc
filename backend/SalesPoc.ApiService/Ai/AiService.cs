using System.Text.Json;
using Microsoft.Extensions.AI;
using SalesToolPoc.ApiService.Ai.Client;
using SalesToolPoc.ApiService.Models;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace SalesToolPoc.ApiService.Ai;

public interface IAiService
{
    public Task<RequestSummary> GetRequestSummary(string emailBody, CancellationToken cancellationToken = default);
}

public class AiService : IAiService
{
    private readonly IChatClient _apiClient;

    private const string JsonSchema = """
                                      {
                                        "type": "object",
                                        "properties": {
                                          "title": {
                                            "type": "string"
                                          },
                                          "summary": {
                                            "type": "string"
                                          },
                                          "role": {
                                            "type": "string"
                                          },
                                          "customer": {
                                            "type": "string"
                                          },
                                          "deadline": {
                                            "type": "string"
                                          }
                                        },
                                        "required": [
                                          "title","summary","role","customer"
                                        ]
                                      }
                                      """;

    public AiService()
    {
        _apiClient = new StructuredOllamaClient("http://localhost:11434", "gemma2");
    }

    public async Task<RequestSummary> GetRequestSummary(string emailBody, CancellationToken cancellationToken)
    {
        ChatMessage[] messages =
        [
            new(ChatRole.System, "You are summarizing requests for consultants. Reply in Norwegian. Reply using JSON. The summary should be max 100 words. The deadline should be a iso8601 utc string. If you are unsure about a field, use a 'null' value."),
            new(ChatRole.User, emailBody)
        ];
        
        var response = await _apiClient.CompleteAsync(messages, new ChatOptions
        {
            Temperature = 0,
            ResponseFormat = new ChatResponseFormatJson(JsonSchema),
        }, cancellationToken);

        var summary = JsonSerializer.Deserialize<RequestSummary>(response.Message.Text, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return summary;  
    }
}
