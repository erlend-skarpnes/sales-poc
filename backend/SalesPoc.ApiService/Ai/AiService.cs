using System.Text.Json;
using Microsoft.Extensions.AI;
using SalesToolPoc.ApiService.Ai.Client;
using SalesToolPoc.ApiService.Models;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace SalesToolPoc.ApiService.Ai;

public interface IAiService
{
    public Task<RequestSummary> GetRequestSummary(string contentBody, CancellationToken cancellationToken = default);
}

public class AiService(IChatClient client) : IAiService
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

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

    public async Task<RequestSummary> GetRequestSummary(string contentBody, CancellationToken cancellationToken)
    {
        ChatMessage[] messages =
        [
            new(ChatRole.System,
                "You are summarizing requests for consultants. Reply in Norwegian. Reply using JSON. The summary should be max 100 words. The deadline should be a iso8601 utc string. If you are unsure about a field, use a 'null' value."),
            new(ChatRole.User, contentBody)
        ];

        var response = await client.CompleteAsync(messages, new ChatOptions
        {
            Temperature = 0,
            ResponseFormat = new ChatResponseFormatJson(JsonSchema),
        }, cancellationToken);

        var summary = JsonSerializer.Deserialize<RequestSummary>(response.Message.Text, Options);

        return summary;
    }
}